import sys, time,threading
from MCP3008 import *
from Buzzer import Buzzer
from APICaller import APICaller

apiCaller = APICaller()
alarmTriggered = False

def startAlarm():
		try:
				alarmTriggered = False
				print("Starting alarm..")
				buzzer = Buzzer()
				adc = MCP3008()
				while (not alarmTriggered):
						#print(adc.read(0))
						if(adc.read(0)>1000):
								alarmTriggered=True
								apiCaller.setStatus()
								buzzer.startBuzzer()

		except:
				print("\nUnknown error")

def registerDevice():
		alarmTriggered = False
		apiCaller.register()
		startAlarm()



def getStatus():	
	buzzer = Buzzer()
	if(apiCaller.getStatus()['triggered']):
		buzzer.startBuzzer()
	threading.Timer(5,getStatus).start()

def setStatus():
	apiCaller.setStatus()



getStatus()

registerDevice()
setStatus()

