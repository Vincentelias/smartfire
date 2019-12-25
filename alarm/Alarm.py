# import sys, time,threading
from MCP3008 import MCP3008
import sys
import Adafruit_DHT
from Buzzer import Buzzer
from APICaller import APICaller
from time import sleep
import asyncio
import RPi.GPIO as GPIO
import threading
import json
from azure.iot.device.aio import IoTHubDeviceClient

#for DHT11 temperature sensor
GPIO.setmode(GPIO.BCM)
GPIO.setup(23, GPIO.OUT)


class Alarm:

	gas_threshold=1000
	smoke_threshold=2000
	temperature_threshold=55

	def __init__(self):
		self.fire_detected=False
		self.apiCaller=APICaller()
		self.buzzer=Buzzer()
		self.adc = MCP3008()


	async def connect_azure(self):
		conn_str = "HostName=Smartfire.azure-devices.net;DeviceId=smartfire-1;SharedAccessKey=sFQIzn79jON2ZBpL3iiBHScmIBXMXf57bA5PFM69ie8="
		self.device_client = IoTHubDeviceClient.create_from_connection_string(conn_str)
		await self.device_client.connect()
		print("connected to iot hub")


	async def start(self):
		await self.connect_azure()
		await self.start_alarm()


	async def start_alarm(self):
		while True:
			humidity,temperature=self.get_dht11_data()
			mq3_data=self.get_mq3_data()
			mq9_data=self.get_mq9_data()
			await self.save_measurements(temperature,humidity,mq3_data,mq9_data)


			if((mq3_data>Alarm.gas_threshold or mq9_data>Alarm.smoke_threshold or temperature>Alarm.temperature_threshold) and not self.fire_detected):
				self.fire_detected=True
				#runs blocking buzzer code in different thread
				buzzer_thread = threading.Thread(target=self.buzzer.start)
				buzzer_thread.start()


	async def save_measurements(self,temperature,humidity,mq3_data,mq9_data):
		print("saving measurements")

		gas_percentage=self.convert_percentage(mq3_data,350,1000)
		co_percentage=self.convert_percentage(mq3_data,990,5000)
		print("gas: "+str(mq3_data))
		print("Temperature: "+str(temperature)+"	Humidity: "+str(humidity)+"	gas percentage: "+str(gas_percentage)+"%	o percentage: "+str(co_percentage)+"%")
		
		data = {
			'device_id': 1,
			'Event': "measurement",
			'temperature': float(temperature),
			'gas_percentage':gas_percentage,
			'co_percentage':co_percentage,
			'is_fire':1 if self.fire_detected else 0,
			'humidity':float(humidity)
		}

		json_body = json.dumps(data)
		print("Sending message: ", json_body)
		await self.device_client.send_message(json_body)
		twin = await self.device_client.get_twin()
		self.handle_twin(twin)

		await self.device_client.disconnect()


	def handle_twin(self,twin):
		print("Twin received", twin)
		if ('desired' in twin):
			desired = twin['desired']
			if ('is_fire' in desired):
				if desired['is_fire']==1:
					buzzer_thread = threading.Thread(target=self.buzzer.start)
					buzzer_thread.start()


	def convert_percentage(self,number,min,max):
		percentage = round(((number-min)/(max-min))*100,2)
		if percentage > 100:
			percentage=100
		elif percentage<0:
			percentage=0
		return float(percentage)

	def get_dht11_data(self):
		return Adafruit_DHT.read_retry(11, 4)

	def get_mq3_data(self):
		return self.adc.read(0)

	def get_mq9_data(self):
		return self.adc.read(1)


