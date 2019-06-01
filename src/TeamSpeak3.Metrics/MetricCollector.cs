using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeak3.Metrics.v2;

namespace TeamSpeak3.Metrics
{
    public class MetricCollector
    {
        private readonly Gateway _gateway;

        public MetricCollector(Gateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IEnumerable<Teamspeak3Metrics>> Collect()
        {
            var servers = await _gateway.GetServerList();
            foreach (var server in servers)
            {
                var clients = await _gateway.GetClientList(server.VirtualServerPort);
                var info = await _gateway.GetServerInfo(server.VirtualServerPort);
            }

            return Enumerable.Empty<Teamspeak3Metrics>();
        }
    }
}
