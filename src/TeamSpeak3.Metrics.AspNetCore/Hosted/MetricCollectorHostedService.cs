using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics.AspNetCore.Hosted
{
    public class MetricCollectorHostedService : BackgroundService, IMetricCollectorCache
    {
        private readonly IMetricCollector _metricCollector;
        private readonly MetricCollectorHostedServiceOptions _options;

        public MetricCollectorHostedService(IMetricCollector metricCollector, IOptions<MetricCollectorHostedServiceOptions> options)
        {
            _metricCollector = metricCollector;
            _options = options.Value;
        }

        public IEnumerable<TeamSpeak3Metrics> Current { get; private set; }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Current = await _metricCollector.Collect();
                await Task.Delay(_options.Delay, cancellationToken);
            }
        }
    }
}
