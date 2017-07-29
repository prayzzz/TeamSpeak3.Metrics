using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Query;
using TeamSpeak3.Metrics.Query.Data;

namespace TeamSpeak3.Metrics
{
    public class PeriodicDataCollector : IDisposable
    {
        private readonly ILogger<PeriodicDataCollector> _logger;
        private readonly Task<TeamSpeakQuery> _teamspeak;
        private readonly Timer _timer;

        public PeriodicDataCollector(ILogger<PeriodicDataCollector> logger, Task<TeamSpeakQuery> teamspeak)
        {
            _logger = logger;
            _teamspeak = teamspeak;

            _timer = new Timer(Collect, this, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public IEnumerable<Client> Clients { get; set; } = new List<Client>();

        public VirtualServer VirtualServer { get; set; } = new VirtualServer();

        public void Dispose()
        {
            _timer.Dispose();
        }

        private static async void Collect(object state)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var collector = (PeriodicDataCollector) state;
            var query = await collector._teamspeak;

            var clients = await query.ClientList();
            var serverInfo = await query.ServerInfo();

            if (clients.HasError)
            {
                collector._logger.LogError("Error while requesting clients: {ErrorId} {ErrorMessage}", clients.ErrorId, clients.ErrorMessage);
            }
            else
            {
                collector.Clients = clients.Data;
            }

            if (serverInfo.HasError)
            {
                collector._logger.LogError("Error while requesting serverinfo: {ErrorId} {ErrorMessage}", serverInfo.ErrorId, serverInfo.ErrorMessage);
            }
            else
            {
                collector.VirtualServer = serverInfo.Data;
            }

            stopwatch.Stop();
            collector._logger.LogDebug("Data collected in {Elapsed}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}