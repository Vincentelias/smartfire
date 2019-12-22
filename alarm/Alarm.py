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
		self.start_alarm()


	def start_alarm(self):
		while True:
			temperature=self.get_temperature()
			mq3_data=self.get_mq3_data()
			mq9_data=self.get_mq9_data()
			self.save_measurements(temperature,mq3_data,mq9_data)


			if((mq3_data>Alarm.gas_threshold or mq9_data>Alarm.smoke_threshold or temperature>Alarm.temperature_threshold) and not self.fire_detected):
				self.fire_detected=True
				#runs blocking buzzer code in different thread
				buzzer_thread = threading.Thread(target=self.buzzer.start)
				buzzer_thread.start()


	def save_measurements(self,temperature,mq3_data,mq9_data):
		print("saving measurements")

		gas_percentage=self.convert_percentage(mq3_data,230,1000)
		co_percentage=self.convert_percentage(mq3_data,990,5000)
		
			# data = {
			# 	'device_id': 1,
			# 	'Event': "measurement",
			# 	'temperature': float(temp),

			# }

			# json_body = json.dumps(data)
			# print("Sending message: ", json_body)
			# await device_client.send_message(json_body)
			# twin = await device_client.get_twin()
			# handle_twin(twin)

		#await device_client.disconnect()

	def convert_percentage(self,number,min,max):
		percentage = round(((number-min)/(max-min))*100,2)
		if percentage > 100:
			return 100
		elif percentage<0:
			return 0
		else:
			return percentage

	def get_temperature(self):
		humidity,temperature = Adafruit_DHT.read_retry(11, 4)
		return temperature

	def get_mq3_data(self):
		return self.adc.read(0)

	def get_mq9_data(self):
		return self.adc.read(1)


