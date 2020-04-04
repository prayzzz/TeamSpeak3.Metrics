using System.Collections.Generic;

namespace TeamSpeak3.Metrics.Models
{
    public class TeamSpeak3Metrics
    {
        public int ChannelsOnline { get; set; }

        public int ClientConnections { get; set; }

        public IEnumerable<string> Clients { get; set; }

        public int ClientsOnline { get; set; }

        public ulong ConnectionBytesReceivedSpeech { get; set; }

        public ulong ConnectionBytesReceivedTotal { get; set; }

        public ulong ConnectionBytesSentSpeech { get; set; }

        public ulong ConnectionBytesSentTotal { get; set; }

        public ulong ConnectionFiletransferBytesReceivedTotal { get; set; }

        public ulong ConnectionFiletransferBytesSentTotal { get; set; }

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
