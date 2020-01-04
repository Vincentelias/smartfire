import RPi.GPIO as GPIO
from time import sleep
import threading

class Buzzer:

	t=""
	is_buzzing=False

	def __init__(self):
		GPIO.setmode(GPIO.BCM)
		GPIO.setup(23,GPIO.OUT)
		self.p=GPIO.PWM(23,5000)


	def stop(self):
		if self.is_buzzing:
			self.t.do_run = False
			self.t.join()
			self.is_buzzing=False
		print("stopped")

	def start(self):
		if not(self.is_buzzing):
			self.t=threading.Thread(target=self.start_different_thread, args=("task",))
			self.t.start()
			self.is_buzzing=True


	def start_different_thread(self,args):
		
		t=threading.currentThread()
		print("starting buzzer")
		while getattr(t,"do_run",True):
			self.turn_on_for(50,1000,0.1)
			self.turn_on_for(50,2000,0.1)
			self.turn_off_for(.05)
		print("stopping buzzer")


	def turn_on_for(self,duty_cycle,frequency,duration):
		self.p.ChangeFrequency(frequency)
		self.p.start(duty_cycle)
		sleep(duration)


	def turn_off_for(self,duration):
		self.p.stop()
		sleep(duration)
