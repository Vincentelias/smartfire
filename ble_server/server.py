#!/usr/bin/python3

import dbus
import fcntl
import socket
import struct
from advertisement import Advertisement
from service import Application, Service, Characteristic, Descriptor

GATT_CHRC_IFACE = "org.bluez.GattCharacteristic1"
NOTIFY_TIMEOUT = 5000

class MacAdvertisement(Advertisement):
    def __init__(self, index):
        Advertisement.__init__(self, index, "peripheral")
        self.add_local_name("Smartfire-1")
        self.include_tx_power = True


class MacService(Service):
    MAC_SVC_UUID = "00000001-710e-4a5b-8d75-3e5b444bc3cf"

    def __init__(self, index):

        Service.__init__(self, index, self.MAC_SVC_UUID, True)
        self.add_characteristic(MacCharacteristic(self))


class MacCharacteristic(Characteristic):
    MAC_CHARACTERISTIC_UUID = "00000002-710e-4a5b-8d75-3e5b444bc3cf"

    def __init__(self, service):
        self.notifying = False

        Characteristic.__init__(
            self, self.MAC_CHARACTERISTIC_UUID,
            ["notify", "read"], service)
        self.add_descriptor(MacDescriptor(self))

    def get_mac(self):
        value = []
        ifname = "wlan0"
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        info = fcntl.ioctl(s.fileno(), 0x8927,
                           struct.pack('256s', ifname[:15]))
        mac = ':'.join(['%02x' % ord(char) for char in info[18:24]])
        for c in mac:
            value.append(dbus.Byte(c.encode()))

        return value

    def set_mac_callback(self):
        if self.notifying:
            value = self.get_mac()
            self.PropertiesChanged(GATT_CHRC_IFACE, {"Value": value}, [])

        return self.notifying

    def StartNotify(self):
        if self.notifying:
            return
    
        self.notifying = True

        value = self.get_mac()
        self.PropertiesChanged(GATT_CHRC_IFACE, {"Value": value}, [])
        self.add_timeout(NOTIFY_TIMEOUT, self.set_mac_callback)

    def StopNotify(self):
        self.notifying = False

    def ReadValue(self, options):
        value = self.get_mac()

        return value


class MacDescriptor(Descriptor):
    MAC_DESCRIPTOR_UUID = "2901"
    MAC_DESCRIPTOR_VALUE = "MAC Address"

    def __init__(self, characteristic):
        Descriptor.__init__(
            self, self.MAC_DESCRIPTOR_UUID,
            ["read"],
            characteristic)

    def ReadValue(self, options):
        value = []
        desc = self.MAC_DESCRIPTOR_VALUE

        for c in desc:
            value.append(dbus.Byte(c.encode()))

        return value

if __name__=="__main__":
    app = Application()
    app.add_service(MacService(0))
    app.register()

    adv = MacAdvertisement(0)
    adv.register()


    app.run()
        