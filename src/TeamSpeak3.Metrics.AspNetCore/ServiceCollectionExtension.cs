using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TeamSpeak3.Metrics.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        public static ITeamSpeak3MetricsBuilder AddTeamSpeak3Metrics(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IQueryConnectionFactory, QueryConnectionFactory>();
            serviceCollection.TryAddSingleton<IMetricCollector, MetricCollector>();
            serviceCollection.TryAddSingleton<IGateway, Gateway>();

            return new TeamSpeak3MetricsBuilder(serviceCollection);
        }
    }
}
