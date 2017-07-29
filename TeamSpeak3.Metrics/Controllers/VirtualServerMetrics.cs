using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Controllers
{
    public class VirtualServerMetrics
    {
        public int ClientsOnline { get; set; }

        public string ServerName { get; set; }

        public IEnumerable<string> Clients { get; set; }

        public int BytesSent { get; set; }

        public int BytesReceived { get; set; }

        public int ServerId { get; set; }

        public string Status { get; set; }

        public string Uptime { get; set; }
    }
}