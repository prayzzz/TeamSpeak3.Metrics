using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Query.Data;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics.Query
{
    public class TeamSpeakQuery : IDisposable
    {
        private const string ClientlistCommand = "clientlist";
        private const string ServerInfoCommand = "serverinfo";

        private readonly ILogger<TeamSpeakQuery> _logger;

        public TeamSpeakQuery(ILogger<TeamSpeakQuery> logger)
        {
            _logger = logger;
        }

        public TelnetClient TelnetClient { get; private set; }

        public async Task<Response<List<Client>>> ClientList()
        {
            await TelnetClient.WriteLine(ClientlistCommand);
            var response = await TelnetClient.ReadAsync();

            _logger.LogDebug("Executed {Command}. Received {Response}", ClientlistCommand, response);

            return new Response<List<Client>>(response);
        }

        public async Task<Response<VirtualServer>> ServerInfo()
        {
            await TelnetClient.WriteLine(ServerInfoCommand);
            var response = await TelnetClient.ReadAsync();

            _logger.LogDebug("Executed {Command}. Received {Response}", ServerInfoCommand, response);

            return new Response<VirtualServer>(response);
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
            var command = $"login {username} {password}";

            await TelnetClient.WriteLine(command);
            var response = await TelnetClient.ReadAsync();

            _logger.LogDebug("Executed {Command}. Received {Response}", command, response);

            return new Response<VirtualServer>(response);
        }

        public async Task<Response> Use(int port)
        {
            var command = $"use port={port}";

            await TelnetClient.WriteLine(command);
            var response = await TelnetClient.ReadAsync();

            _logger.LogDebug("Executed {Command}. Received {Response}", command, response);

            return new Response<VirtualServer>(response);
        }
    }
}