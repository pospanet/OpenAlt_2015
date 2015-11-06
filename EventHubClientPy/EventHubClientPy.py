from azure.servicebus import ServiceBusService
import psutil

key_name = '<access_key_name>' # SharedAccessKeyName from Azure portal
key_value = '<access_key_value>' # SharedAccessKey from Azure portal
sb_namespace = '<namespace_name>'
sbs = ServiceBusService(service_namespace = sb_namespace,
                        shared_access_key_name=key_name,
                        shared_access_key_value=key_value)
# sbs.create_event_hub('openalt')

count = 0;

while True:
    sbs.send_event('<eventhub_name>', '{ "MachineName":"Python-01", "CpuUsage":"'+str(psutil.cpu_percent())+'", "FreeMem":"" }')
    print(count)
    print('CpuUsage: '+str(psutil.cpu_percent()))
    count = count + 1
