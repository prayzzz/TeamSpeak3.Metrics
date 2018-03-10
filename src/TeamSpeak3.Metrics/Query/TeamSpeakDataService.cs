using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Model;

namespace TeamSpeak3.Metrics.Query
{
    public interface ITeamSpeakMetrics
    {
        bool IsError { get; }

        VirtualServerMetrics Metrics { get; }
    }

    public class TeamSpeakDataService : HostedService, ITeamSpeakMetrics
    {
        private readonly TeamSpeakConfiguration _configuration;
        private readonly Func<TeamSpeakConnection> _connectionProvider;
        private readonly ILogger<TeamSpeakDataService> _logger;

        public TeamSpeakDataService(Func<TeamSpeakConnection> connectionProvider,
                                    IOptions<AppConfiguration> options,
                                    ILogger<TeamSpeakDataService> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
            _configuration = options.Value.TeamSpeak;
        }

        public bool IsError { get; private set; }

        public VirtualServerMetrics Metrics { get; private set; }

        protected override ILogger Logger => _logger;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Collect();
                }
                catch (Exception e)
                {
                    Logger.LogError($"Collect failed: {e.Message}", e);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }

        private async Task Collect()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var collectedMetrics = new VirtualServerMetrics();
            using (var teamspeak = _connectionProvider())
            {
                if (!await teamspeak.Connect(_configuration.Ip, _configuration.QueryPort))
                {
                    IsError = true;
                    return;
                }

                await teamspeak.Login(_configuration.QueryUsername, _configuration.QueryPassword);
                await teamspeak.Use(_configuration.Port);

                var collectClientListTask = CollectClientList(teamspeak, collectedMetrics);
                var collectServerInfoTask = CollectServerInfo(teamspeak, collectedMetrics);

                await collectClientListTask;
                await collectServerInfoTask;
            }

            stopwatch.Stop();
            _logger.LogDebug("Data collected in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            collectedMetrics.CollectedAt = DateTime.Now;
            collectedMetrics.CollectionDuration = stopwatch.ElapsedMilliseconds;

            IsError = false;
            Metrics = collectedMetrics;
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
                collectedMetrics.Uptime = long.Parse(response.Data.VirtualServerUptime);
            }
            else
            {
                _logger.LogError("Error while requesting serverinfo: {ErrorId} {ErrorMessage}", response.ErrorId, response.ErrorMessage);
            }
        }
    }
}