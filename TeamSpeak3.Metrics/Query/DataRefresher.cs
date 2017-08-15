using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Query.Data;

namespace TeamSpeak3.Metrics.Query
{
    public class DataRefresher : HostedService
    {
        private readonly Func<TeamSpeakConnection> _connectionProvider;
        private readonly TeamSpeakData _data;
        private readonly ILogger<DataRefresher> _logger;
        private readonly TeamSpeakSettings _settings;

        public DataRefresher(Func<TeamSpeakConnection> connectionProvider,
                             TeamSpeakData data,
                             IOptions<AppSettings> options,
                             ILogger<DataRefresher> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
            _data = data;
            _settings = options.Value.TeamSpeak;
        }

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

            Response<List<Client>> clients;
            Response<VirtualServer> serverInfo;
            using (var teamspeak = _connectionProvider())
            {
                await teamspeak.Connect(_settings.Ip, _settings.QueryPort);
                await teamspeak.Login(_settings.QueryUsername, _settings.QueryPassword);
                await teamspeak.Use(_settings.Port);

                clients = await teamspeak.ClientList();
                serverInfo = await teamspeak.ServerInfo();
            }

            if (clients.HasError)
            {
                _logger.LogError("Error while requesting clients: {ErrorId} {ErrorMessage}", clients.ErrorId, clients.ErrorMessage);
            }
            else
            {
                _data.Clients = clients.Data;
            }

            if (serverInfo.HasError)
            {
                _logger.LogError("Error while requesting serverinfo: {ErrorId} {ErrorMessage}", serverInfo.ErrorId, serverInfo.ErrorMessage);
            }
            else
            {
                _data.VirtualServer = serverInfo.Data;
            }

            stopwatch.Stop();
            _logger.LogDebug("Data collected in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            _data.CollectedAt = DateTime.Now;
            _data.CollectionDuration = stopwatch.ElapsedMilliseconds;
        }
    }
}