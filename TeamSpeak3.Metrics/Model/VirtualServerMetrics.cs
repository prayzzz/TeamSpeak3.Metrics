using System;
using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Model
{
    public class VirtualServerMetrics
    {
        public int BytesReceived { get; set; }

        public int BytesSent { get; set; }

        public IEnumerable<string> Clients { get; set; }

        public int ClientsOnline { get; set; }

        public DateTime CollectedAt { get; set; }

        public long CollectionDuration { get; set; }

        public int ServerId { get; set; }

        public string ServerName { get; set; }

        public string Status { get; set; }

        public double TotalPing { get; set; }

        public string Uptime { get; set; }
    }
}