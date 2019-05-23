using System;
using System.Threading;
using System.Threading.Tasks;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics.v2
{
    public interface IQueryConnectionFactory
    {
        Task<IQueryConnection> Create(string ip, int port);
    }

    public class QueryConnectionFactory : IQueryConnectionFactory
    {
        public async Task<IQueryConnection> Create(string ip, int port)
        {
            if (string.IsNullOrEmpty(ip)) throw new ArgumentNullException(nameof(ip));

            var cancellationToken = new CancellationToken();

            TelnetClient telnetClient;
            try
            {
                telnetClient = new TelnetClient(ip, port, cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                throw new TeamSpeak3MetricsException($"Couldn't connect to TeamSpeak server at {ip}:{port}", e);
            }
            catch (Exception e)
            {
                throw new TeamSpeak3MetricsException($"Unknown error while establishing connect to TeamSpeak server at {ip}:{port}", e);
            }

            if (!telnetClient.IsConnected)
            {
                throw new TeamSpeak3MetricsException($"Couldn't connect to TeamSpeak server at {ip}:{port}");
            }

            // Read welcome message
            var response = string.Empty;
            while (string.IsNullOrEmpty(response))
            {
                response = (await telnetClient.ReadAsync()).Trim();
            }

            return new QueryConnection(telnetClient);
        }
    }
}
