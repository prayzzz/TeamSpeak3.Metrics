using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TeamSpeak3.Metrics.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTeamSpeak3Metrics(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IQueryConnectionFactory, QueryConnectionFactory>();
            serviceCollection.TryAddSingleton<IMetricCollector, MetricCollector>();
            serviceCollection.TryAddSingleton<IGateway, Gateway>();

            return serviceCollection;
        }
    }
}
