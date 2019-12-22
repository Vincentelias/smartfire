import RPi.GPIO as GPIO
from time import sleep
import asyncio

class Buzzer:

	def __init__(self):
		self.is_on=False
		GPIO.setmode(GPIO.BCM)
		GPIO.setup(23,GPIO.OUT)
		self.p=GPIO.PWM(23,5000)

	def start(self):
		print("starting buzzer..")
		self.is_on=True
		while self.is_on:
			self.turn_on_for(10,1000,.08)
			self.turn_on_for(10,2000,.08)
			self.turn_off_for(.01)
			
	def pause(self,seconds):
		self.turn_off()
		sleep(seconds)

	def stop(self):
		self.is_on=False
		GPIO.cleanup()

	def turn_on_for(self,duty_cycle,frequency,duration):
		self.p.ChangeFrequency(frequency)
		self.p.start(duty_cycle)
		sleep(duration)

	def turn_off_for(self,duration):
		self.p.stop()
		sleep(duration)
