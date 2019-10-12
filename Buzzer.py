import RPi.GPIO as GPIO
from time import sleep

class Buzzer:
	def startBuzzer(self):
		print("starting buzzer 0")
		GPIO.setmode(GPIO.BCM)
		GPIO.setup(23,GPIO.OUT)
		p=GPIO.PWM(23,10190)
		print("starting buzzer..")
		p.start(50.0)
		sleep(1000000)
	def stop():
		GPIO.cleanup()
