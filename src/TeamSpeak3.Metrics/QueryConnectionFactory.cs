using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Common;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics
{
    public interface IQueryConnectionFactory
    {
        Task<IQueryConnection> Create(string ip, int port);
    }

    public class QueryConnectionFactory : IQueryConnectionFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public QueryConnectionFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task<IQueryConnection> Create(string ip, int port)
        {
            if (string.IsNullOrEmpty(ip))
            {
                throw new ArgumentNullException(nameof(ip));
            }

            var cancellationToken = new CancellationToken();

            TelnetClient telnetClient;
            try
            {
                telnetClient = new TelnetClient(ip, port, cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                throw new MetricsException($"Couldn't connect to TeamSpeak server at {ip}:{port}", e);
            }
            catch (Exception e)
            {
                throw new MetricsException($"Unknown error while establishing connect to TeamSpeak server at {ip}:{port}", e);
            }

            if (!telnetClient.IsConnected)
            {
                throw new MetricsException($"Couldn't connect to TeamSpeak server at {ip}:{port}");
            }

            // Read welcome message
            var response = string.Empty;
            while (string.IsNullOrEmpty(response))
            {
                response = (await telnetClient.ReadAsync()).Trim();
            }

            return new QueryConnection(telnetClient, _loggerFactory.CreateLogger<QueryConnection>());
        }
    }
}
