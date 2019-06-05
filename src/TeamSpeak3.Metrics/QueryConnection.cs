using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics
{
    public interface IQueryConnection : IDisposable
    {
        Task<string> SendAndReceive(string command);
    }

    internal class QueryConnection : IQueryConnection
    {
        private readonly ILogger<QueryConnection> _logger;
        private TelnetClient _telnetClient;

        internal QueryConnection(TelnetClient telnetClient, ILogger<QueryConnection> logger)
        {
            _telnetClient = telnetClient;
            _logger = logger;
        }

        public void Dispose()
        {
            _telnetClient?.Dispose();
            _telnetClient = null;
        }

        public async Task<string> SendAndReceive(string command)
        {
            await _telnetClient.WriteLine(command);
            var response = (await _telnetClient.ReadAsync()).Trim();

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                if (!command.StartsWith("login"))
                {
                    _logger.LogDebug($"Command: {command}");
                }
                else
                {
                    _logger.LogDebug("Command: login");
                }

                _logger.LogDebug($"Response: {response}");
            }

            return response;
        }
    }
}
