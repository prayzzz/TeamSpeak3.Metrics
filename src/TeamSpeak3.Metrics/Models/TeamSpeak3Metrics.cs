using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Models
{
    public class TeamSpeak3Metrics
    {
        public int ChannelsOnline { get; set; }

        public int ClientConnections { get; set; }

        public IEnumerable<string> Clients { get; set; }

        public int ClientsOnline { get; set; }

        public int ConnectionBytesReceivedSpeech { get; set; }

        public int ConnectionBytesReceivedTotal { get; set; }

        public int ConnectionBytesSentSpeech { get; set; }

        public int ConnectionBytesSentTotal { get; set; }

        public int ConnectionFiletransferBytesReceivedTotal { get; set; }

        public int ConnectionFiletransferBytesSentTotal { get; set; }

        public int MaxClients { get; set; }

        public string Name { get; set; }

        public int Port { get; set; }

        public int QueryClientConnections { get; set; }

        public int QueryClientsOnline { get; set; }

        public int ServerId { get; set; }

        public string Status { get; set; }

        public int TotalBytesDownloaded { get; set; }

        public int TotalBytesUploaded { get; set; }

        public double TotalPacketlossTotal { get; set; }

        public double TotalPing { get; set; }

        public int Uptime { get; set; }
    }
}
