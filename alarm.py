import sys, time
from MCP3008 import *
from Buzzer import Buzzer
from APICaller import *

apiCaller = APICaller()

def startAlarm():
    try:
    print("Starting alarm..")
    buzzer=Buzzer()
    adc = MCP3008()
    alarmTriggered=False
    while (not alarmTriggered): 
	print(adc.read(0))
        if(adc.read(0)>1000):
            alarmTriggered=True 
            buzzer.startBuzzer()

    except:
        print("\nUnknown error")

def register():
    print("registring")
    startAlarm()
    
