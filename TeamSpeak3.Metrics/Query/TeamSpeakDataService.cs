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
        VirtualServerMetrics Metrics { get; }
    }

    public class TeamSpeakDataService : HostedService, ITeamSpeakMetrics
    {
        private readonly Func<TeamSpeakConnection> _connectionProvider;
        private readonly ILogger<TeamSpeakDataService> _logger;
        private readonly TeamSpeakSettings _settings;

        public TeamSpeakDataService(Func<TeamSpeakConnection> connectionProvider,
                                    IOptions<AppSettings> options,
                                    ILoggerFactory factory,
                                    ILogger<TeamSpeakDataService> logger)
        {
            _logger = factory.CreateLogger<TeamSpeakDataService>();
            _connectionProvider = connectionProvider;
            _settings = options.Value.TeamSpeak;
        }

        public VirtualServerMetrics Metrics { get; private set; }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Collect();
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
                await teamspeak.Connect(_settings.Ip, _settings.QueryPort);
                await teamspeak.Login(_settings.QueryUsername, _settings.QueryPassword);
                await teamspeak.Use(_settings.Port);

                await CollectClientList(teamspeak, collectedMetrics);
                await CollectServerInfo(teamspeak, collectedMetrics);
            }

            stopwatch.Stop();
            _logger.LogDebug("Data collected in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            collectedMetrics.CollectedAt = DateTime.Now;
            collectedMetrics.CollectionDuration = stopwatch.ElapsedMilliseconds;

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
                collectedMetrics.Uptime = response.Data.VirtualServerUptime;
            }
            else
            {
                _logger.LogError("Error while requesting serverinfo: {ErrorId} {ErrorMessage}", response.ErrorId, response.ErrorMessage);
            }
        }
    }
}