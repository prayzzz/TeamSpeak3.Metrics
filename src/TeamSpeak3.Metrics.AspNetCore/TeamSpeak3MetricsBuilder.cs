using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TeamSpeak3.Metrics.AspNetCore.Hosted;

namespace TeamSpeak3.Metrics.AspNetCore
{
    public interface ITeamSpeak3MetricsBuilder
    {
        ITeamSpeak3MetricsBuilder AsHostedService();

        ITeamSpeak3MetricsBuilder AsHostedService(TimeSpan delay);
    }

    internal class TeamSpeak3MetricsBuilder : ITeamSpeak3MetricsBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        internal TeamSpeak3MetricsBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        /// <summary>
        ///     Adds <see cref="MetricCollectorHostedService" /> with a delay of 30 seconds
        /// </summary>
        public ITeamSpeak3MetricsBuilder AsHostedService()
        {
            return AsHostedService(TimeSpan.FromSeconds(30));
        }

        public ITeamSpeak3MetricsBuilder AsHostedService(TimeSpan delay)
        {
            _serviceCollection.TryAddSingleton<MetricCollectorHostedService>();
            _serviceCollection.TryAddSingleton<IMetricCollectorCache>(s => s.GetRequiredService<MetricCollectorHostedService>());
            _serviceCollection.TryAddSingleton<IHostedService>(s => s.GetRequiredService<MetricCollectorHostedService>());
            _serviceCollection.Configure<MetricCollectorHostedServiceOptions>(o => o.Delay = delay);

            return this;
        }
    }
}
