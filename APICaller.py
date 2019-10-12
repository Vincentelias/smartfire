import json
import requests
import uuid

class APICaller:
	baseUrl="https://europe-west1-smartfire-20a61.cloudfunctions.net/"	
		

	def register(self):
		print("registering")
		mac=self.getMAC()
		response = requests.post(self.baseUrl+'login',data={'mac':mac})
		print(response.status_code)
	
	def getMAC(self):
		try:
			str=open('/sys/class/net/eth0/address').read()
		except:
			str="00:00:00:00:00:00"
		return str[0:17]

	def getStatus(self):
                response = requests.get(self.baseUrl+"getStatus")
                return (response.json())

	def setStatus(self):
		print("Getting status")
		mac = self.getMAC()
		status = requests.post(self.baseUrl+'setStatus',data={'mac':mac, 'triggered': True})
		print(status.status_code)
