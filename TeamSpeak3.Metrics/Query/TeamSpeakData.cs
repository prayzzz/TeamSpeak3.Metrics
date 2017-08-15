using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TeamSpeak3.Metrics.Model;
using TeamSpeak3.Metrics.Query.Data;

namespace TeamSpeak3.Metrics.Query
{
    public class TeamSpeakData
    {
        public IEnumerable<Client> Clients { get; set; } = new List<Client>();

        public DateTime CollectedAt { get; set; }

        public long CollectionDuration { get; set; }

        public VirtualServer VirtualServer { get; set; } = new VirtualServer();

        public string Get()
        {
            var metrics = new VirtualServerMetrics
            {
                BytesSent = VirtualServer.ConnectionBytesSentTotal,
                BytesReceived = VirtualServer.ConnectionBytesReceivedTotal,
                ClientsOnline = VirtualServer.VirtualServerClientsOnline,
                Clients = Clients.Select(x => x.ClientNickname),
                CollectedAt = CollectedAt,
                CollectionDuration = CollectionDuration,
                ServerId = VirtualServer.VirtualServerId,
                ServerName = VirtualServer.VirtualServerName,
                Status = VirtualServer.VirtualServerStatus,
                TotalPing = VirtualServer.VirtualServerTotalPing,
                Uptime = VirtualServer.VirtualServerUptime
            };

            return JsonConvert.SerializeObject(metrics);
        }
    }
}