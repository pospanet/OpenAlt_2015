using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubClientMono
{
    class Program
    {
        private const string ServiceBusName = "<namespace_name>";
        private const string EventHubName = "<eventhub_name>";
        private const string PolicyName = "<access_key_name>";
        private const string PolicyKey = "<access_key_value>";
        private const string Publisher = "DataSender";
        private const int Ttl = 200000;

        private static void Main(string[] args)
        {
            PerformanceCounter performanceCounter1 = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
            PerformanceCounter performanceCounter2 = new PerformanceCounter("Memory", "Available MBytes");
            EventHub eventHub = new EventHub(ServiceBusName, EventHubName, Publisher, PolicyName, PolicyKey, Ttl);
            while (true)
            {
                PerformanceData performanceData = new PerformanceData()
                {
                    DateTime = DateTime.UtcNow,
                    CpuUsage = performanceCounter1.NextValue(),
                    FreeMem = performanceCounter2.NextValue(),
                    MachineName = Environment.MachineName
                };
                Console.Out.Write('#');
                Console.Out.Write(eventHub.SendEventHubEventAsync(performanceData) ? '+' : '-');
            }
        }
    }
}
