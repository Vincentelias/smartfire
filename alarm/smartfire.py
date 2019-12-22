from Alarm import Alarm
from multiprocessing import Process
import asyncio
alarm=Alarm()


if __name__=="__main__":
   asyncio.run(alarm.start())
        
        