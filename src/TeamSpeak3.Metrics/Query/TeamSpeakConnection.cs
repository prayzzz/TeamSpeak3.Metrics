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
        private const string ServerListCommand = "serverlist";

        private readonly ILogger _logger;

        public TeamSpeakConnection(ILogger logger)
        {
            _logger = logger;
        }

        private TelnetClient TelnetClient { get; set; }

        internal Task<QueryResponse<List<Client>>> ClientList()
        {
            return SendAndReceive<List<Client>>(ClientlistCommand);
        }

        public async Task<bool> Connect(string ip, int port)
        {
            var cancellationToken = new CancellationToken();

            try
            {
                TelnetClient = new TelnetClient(ip, port, cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(e, "Cannot establish query connection to TeamSpeak-Server at {Ip}:{Port}", ip, port);
                return false;
            }
            catch (Exception)
            {
                _logger.LogCritical("Unknown error while establishing query connection to TeamSpeak Server at {Ip}:{Port}", ip, port);
                return false;
            }

            if (!TelnetClient.IsConnected)
            {
                _logger.LogWarning("Cannot establish query connection to TeamSpeak-Server at {Ip}:{Port}", ip, port);
                return false;
            }

            // Read welcome message
            var response = string.Empty;
            while (string.IsNullOrEmpty(response))
            {
                response = (await TelnetClient.ReadAsync()).Trim();
            }

            return true;
        }

        public void Dispose()
        {
            TelnetClient?.Dispose();
        }

        internal Task<QueryResponse> Login(string username, string password)
        {
            var command = $"login {username} {password}";

            return SendAndReceive(command, true);
        }

        internal Task<QueryResponse<VirtualServerInfo>> ServerInfo()
        {
            return SendAndReceive<VirtualServerInfo>(ServerInfoCommand);
        }

        internal Task<QueryResponse<List<VirtualServer>>> ServerList()
        {
            return SendAndReceive<List<VirtualServer>>(ServerListCommand);
        }

        internal Task<QueryResponse> Use(int port)
        {
            var command = $"use port={port}";

            return SendAndReceive(command);
        }

        private async Task<QueryResponse<T>> SendAndReceive<T>(string command, bool isPrivate = false) where T : new()
        {
            await TelnetClient.WriteLine(command);

            var response = string.Empty;
            while (string.IsNullOrEmpty(response))
            {
                response = (await TelnetClient.ReadAsync()).Trim();
            }

            if (isPrivate)
            {
                command = command.Split(' ').FirstOrDefault();
            }

            _logger.LogDebug("Executed {Command}. Received {Response}", command, response);

            return new QueryResponse<T>(response);
        }

        private async Task<QueryResponse> SendAndReceive(string command, bool isPrivate = false)
        {
            await TelnetClient.WriteLine(command);
            var response = await TelnetClient.ReadAsync();

            if (isPrivate)
            {
                command = command.Split(' ').FirstOrDefault();
            }

            _logger.LogDebug("Executed {Command}. Received {Response}", command, response.Trim());

            return new QueryResponse(response);
        }
    }
}