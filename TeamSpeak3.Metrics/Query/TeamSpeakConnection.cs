using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Query.Data;
using TelnetClient = PrimS.Telnet.Client;

namespace TeamSpeak3.Metrics.Query
{
    public class TeamSpeakConnection : IDisposable
    {
        private const string ClientlistCommand = "clientlist";
        private const string ServerInfoCommand = "serverinfo";

        private readonly ILogger<TeamSpeakConnection> _logger;

        public TeamSpeakConnection(ILogger<TeamSpeakConnection> logger)
        {
            _logger = logger;
        }

        private TelnetClient TelnetClient { get; set; }

        public Task<Response<List<Client>>> ClientList()
        {
            return SendAndReceive<List<Client>>(ClientlistCommand);
        }

        public async Task<bool> Connect(string ip, int port)
        {
            var cancellationToken = new CancellationToken();
            TelnetClient = new TelnetClient(ip, port, cancellationToken);

            // Read welcome message
            await TelnetClient.ReadAsync();

            return TelnetClient.IsConnected;
        }

        public void Dispose()
        {
            TelnetClient.Dispose();
        }

        public Task<Response> Login(string username, string password)
        {
            var command = $"login {username} {password}";

            return SendAndReceive(command, true);
        }

        public Task<Response<VirtualServer>> ServerInfo()
        {
            return SendAndReceive<VirtualServer>(ServerInfoCommand);
        }

        public Task<Response> Use(int port)
        {
            var command = $"use port={port}";

            return SendAndReceive(command);
        }

        private async Task<Response<T>> SendAndReceive<T>(string command, bool isPrivate = false) where T : new()
        {
            await TelnetClient.WriteLine(command);
            var response = await TelnetClient.ReadAsync();

            if (isPrivate)
            {
                command = command.Split(' ').FirstOrDefault();
            }

            _logger.LogDebug("Executed {Command}. Received {Response}", command, response);

            return new Response<T>(response);
        }

        private async Task<Response> SendAndReceive(string command, bool isPrivate = true)
        {
            await TelnetClient.WriteLine(command);
            var response = await TelnetClient.ReadAsync();

            if (isPrivate)
            {
                command = command.Split(' ').FirstOrDefault();
            }

            _logger.LogDebug("Executed {Command}. Received {Response}", command, response.Trim());

            return new Response(response);
        }
    }
}