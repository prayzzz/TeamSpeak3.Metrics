using System;
using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Model
{
    public class VirtualServerMetrics
    {
        public long BytesReceived { get; set; }

        public long BytesSent { get; set; }

        public IEnumerable<string> Clients { get; set; }

        public long ClientsOnline { get; set; }

        public DateTime CollectedAt { get; set; }

        public long CollectionDuration { get; set; }

        public long ServerId { get; set; }

        public string ServerName { get; set; }

        public string Status { get; set; }

        public double TotalPing { get; set; }

        public long Uptime { get; set; }
    }
}