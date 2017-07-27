using System;
using System.Threading;
using System.Threading.Tasks;
using PrimS.Telnet;

namespace TeamSpeak3.Metrics.Connection
{
    public class TeamspeakConnection : IDisposable
    {
        public Client TelnetClient { get; private set; }

        public TeamspeakConnection()
        {
        }

        public bool Connect(string ip, int port)
        {
            var cancellationToken = new CancellationToken();
            TelnetClient = new Client(ip, port, cancellationToken);

            return TelnetClient.IsConnected;
        }

        public async Task<QueryResponse> Login(string username, string password)
        {
            await TelnetClient.WriteLine($"login {username} {password}");

            return new QueryResponse(await TelnetClient.ReadAsync());
        }

        public async Task<QueryResponse> Use(int id)
        {
            await TelnetClient.WriteLine($"use {id}");

            return new QueryResponse(await TelnetClient.ReadAsync());
        }

        public void Dispose()
        {
            TelnetClient.Dispose();
        }
    }
}