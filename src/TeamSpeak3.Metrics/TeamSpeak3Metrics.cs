using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Configuration;
using TeamSpeak3.Metrics.Model;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics
{
    public class TeamSpeak3Metrics
    {
        private readonly ILogger _logger;
        private readonly ServerConfiguration _serverConfigurations;

        public TeamSpeak3Metrics(ServerConfiguration serverConfigurations, ILogger logger)
        {
            _serverConfigurations = serverConfigurations;
            _logger = logger;
        }

        public async Task<VirtualServerMetrics> Collect(ServerConfiguration serverConfiguration)
        {
            var collectedMetrics = new VirtualServerMetrics();
            using (var teamspeak = new TeamSpeakConnection(_logger))
            {
                if (!await teamspeak.Connect(serverConfiguration.Host, serverConfiguration.QueryPort))
                {
                    return null;
                }

                await teamspeak.Login(serverConfiguration.QueryUsername, serverConfiguration.QueryPassword);
                
                // foreach (var port in serverConfiguration.Ports)
                // {
                //     var useResponse = await teamspeak.Use(port);
                //
                //     await CollectClientList(teamspeak, collectedMetrics);
                //     await CollectServerInfo(teamspeak, collectedMetrics);
                // }
            }

            collectedMetrics.CollectedAt = DateTime.Now;

            return collectedMetrics;
        }
        
        private async Task CollectClientList(TeamSpeakConnection teamspeak, VirtualServerMetrics collectedMetrics)
        {
            var response = await teamspeak.ClientList();

            if (response.IsSuccess)
            {
                collectedMetrics.Clients = response.Data.Select(x => x.ClientNickname);
            }
            else
            {
                _logger.LogError("Error while requesting clients: {ErrorId} {ErrorMessage}", response.ErrorId, response.ErrorMessage);
            }
        }

        private async Task CollectServerInfo(TeamSpeakConnection teamspeak, VirtualServerMetrics collectedMetrics)
        {
            var response = await teamspeak.ServerInfo();

            if (response.IsSuccess)
            {
                collectedMetrics.BytesSent = response.Data.ConnectionBytesSentTotal;
                collectedMetrics.BytesReceived = response.Data.ConnectionBytesReceivedTotal;
                collectedMetrics.ClientsOnline = response.Data.VirtualServerClientsOnline;
                collectedMetrics.ServerId = response.Data.VirtualServerId;
                collectedMetrics.ServerName = response.Data.VirtualServerName;
                collectedMetrics.Status = response.Data.VirtualServerStatus;
                collectedMetrics.TotalPing = response.Data.VirtualServerTotalPing;

                if (!string.IsNullOrEmpty(response.Data.VirtualServerUptime))
                {
                    collectedMetrics.Uptime = long.Parse(response.Data.VirtualServerUptime);
                }
                else
                {
                    _logger.LogInformation("Uptimer: " + response.Data.VirtualServerUptime);
                }
            }
            else
            {
                _logger.LogError("Error while requesting serverinfo: {ErrorId} {ErrorMessage}", response.ErrorId, response.ErrorMessage);
            }
        }
    }
}
