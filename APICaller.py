import requests

response = requests.get("https://us-central1-smartfire-20a61.cloudfunctions.net/getDevices")
print(response.status_code)
print(response.json())
