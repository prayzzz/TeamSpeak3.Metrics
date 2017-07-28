using System;
using System.Threading;
using System.Threading.Tasks;
using TeamSpeak3.Metrics.Query.Data;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics.Query
{
    public class TeamSpeakQuery : IDisposable
    {
        public TelnetClient TelnetClient { get; private set; }

        public async Task<Response<Client>> ClientList()
        {
            await TelnetClient.WriteLine("clientlist");

            return new Response<Client>(await TelnetClient.ReadAsync());
        }

        public bool Connect(string ip, int port)
        {
            var cancellationToken = new CancellationToken();
            TelnetClient = new TelnetClient(ip, port, cancellationToken);

            return TelnetClient.IsConnected;
        }

        public void Dispose()
        {
            TelnetClient.Dispose();
        }

        public async Task<Response> Login(string username, string password)
        {
            await TelnetClient.WriteLine($"login {username} {password}");

            return new Response(await TelnetClient.ReadAsync());
        }

        public async Task<Response> Use(int id)
        {
            await TelnetClient.WriteLine($"use {id}");

            return new Response(await TelnetClient.ReadAsync());
        }
    }
}