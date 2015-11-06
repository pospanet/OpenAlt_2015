using System;

namespace EventHubClient
{
    public class PerformanceData
    {
        public DateTime DateTime { get; set; }

        public float CpuUsage { get; set; }

        public float FreeMem { get; set; }

        public string MachineName { get; set; }
    }
}