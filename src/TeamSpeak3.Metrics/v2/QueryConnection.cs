using System;
using System.Threading.Tasks;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics.v2
{
    public interface IQueryConnection : IDisposable
    {
        Task<string> SendAndReceive(string command);
    }

    internal class QueryConnection : IQueryConnection
    {
        private TelnetClient _telnetClient;

        internal QueryConnection(TelnetClient telnetClient)
        {
            _telnetClient = telnetClient;
        }

        public async Task<string> SendAndReceive(string command)
        {
            await _telnetClient.WriteLine(command);
            return await _telnetClient.ReadAsync();
        }

        public void Dispose()
        {
            _telnetClient?.Dispose();
            _telnetClient = null;
        }
    }
}
