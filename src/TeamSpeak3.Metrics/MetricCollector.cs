using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics
{
    public interface IMetricCollector
    {
        Task<IEnumerable<TeamSpeak3Metrics>> Collect();
    }

    public class MetricCollector : IMetricCollector
    {
        private readonly IGateway _gateway;

        public MetricCollector(IGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IEnumerable<TeamSpeak3Metrics>> Collect()
        {
            var metrics = new List<TeamSpeak3Metrics>();
            var servers = await _gateway.GetServerList();
            foreach (var server in servers)
            {
                var clients = await _gateway.GetClientList(server.VirtualServerPort);
                var info = await _gateway.GetServerInfo(server.VirtualServerPort);

                metrics.Add(MapToMetrics(clients, info));
            }

            return metrics;
        }

        private static TeamSpeak3Metrics MapToMetrics(IEnumerable<Client> clients, ServerInfo info)
        {
            return new TeamSpeak3Metrics
            {
                ChannelsOnline = info.VirtualServerChannelsOnline,
                ClientConnections = info.VirtualServerClientConnections,
                Clients = clients.Select(c => c.ClientNickname),
                ClientsOnline = info.VirtualServerClientsOnline,
                ConnectionBytesReceivedTotal = info.ConnectionBytesReceivedTotal,
                ConnectionBytesSentTotal = info.ConnectionBytesSentTotal,
                MaxClients = info.VirtualServerMaxClients,
                Name = info.VirtualServerName,
                Port = info.VirtualServerPort,
                QueryClientConnections = info.VirtualServerQueryClientConnections,
                QueryClientsOnline = info.VirtualServerQueryClientsOnline,
                ServerId = info.VirtualServerId,
                Status = info.VirtualServerStatus,
                TotalPing = info.VirtualServerTotalPing,
                Uptime = info.VirtualServerUptime
            };
        }
    }
}
