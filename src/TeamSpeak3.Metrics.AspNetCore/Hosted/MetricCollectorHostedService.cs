using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics.AspNetCore.Hosted
{
    public class MetricCollectorHostedService : BackgroundService, IMetricCollectorCache
    {
        private readonly ILogger<MetricCollectorHostedService> _logger;
        private readonly IMetricCollector _metricCollector;
        private readonly MetricCollectorHostedServiceOptions _options;

        public MetricCollectorHostedService(IMetricCollector metricCollector, ILogger<MetricCollectorHostedService> logger, IOptions<MetricCollectorHostedServiceOptions> options)
        {
            _metricCollector = metricCollector;
            _options = options.Value;
            _logger = logger;
        }

        public IEnumerable<TeamSpeak3Metrics> Current { get; private set; }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Current = await _metricCollector.Collect();
                }
                catch (Exception e)
                {
                    Current = Enumerable.Empty<TeamSpeak3Metrics>();
                    _logger.LogError(e, "Exception while collecting Metrics");
                }

                await Task.Delay(_options.Delay, cancellationToken);
            }
        }
    }
}