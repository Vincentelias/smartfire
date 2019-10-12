import sys, time,threading
from MCP3008 import *
from Buzzer import Buzzer
from APICaller import APICaller

apiCaller = APICaller()
alarmTriggered = False

def startAlarm():
		try:
				print("Starting alarm..")
				buzzer = Buzzer()
				adc = MCP3008()
				while (not alarmTriggered):
						print(adc.read(0))
						if(adc.read(0)>1000):
								alarmTriggered=True
								buzzer.startBuzzer()

		except:
				print("\nUnknown error")

def registerDevice():
		alarmTriggered = False
		apiCaller.register()
		startAlarm()



def getStatus():
	print("getting status")
	threading.Timer(0.05,getStatus).start()

registerDevice()
getStatus()

