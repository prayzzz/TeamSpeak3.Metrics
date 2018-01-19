using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseRouter(CreateRouter(app));

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            if (serverAddressesFeature != null)
            {
                _logger.LogInformation("Application listening on: {Url}", string.Join(", ", serverAddressesFeature.Addresses));
            }
        }

        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            base.CreateServiceProvider(services);

            services.Configure<AppConfiguration>(_configuration.GetSection("App"));
            services.AddRouting();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<TeamSpeakDataService>().As<IHostedService>().As<ITeamSpeakMetrics>().SingleInstance();
            builder.Register(c =>
            {
                var logger = c.Resolve<ILogger<TeamSpeakConnection>>();
                return new Func<TeamSpeakConnection>(() => new TeamSpeakConnection(logger));
            }).SingleInstance();

            return new AutofacServiceProvider(builder.Build());
        }

        private static IRouter CreateRouter(IApplicationBuilder app)
        {
            var builder = new RouteBuilder(app);
            builder.MapGet("", context =>
            {
                context.Response.Redirect("/api/metrics");
                return Task.CompletedTask;
            });

            builder.MapGet("api/metrics", MetricsRequest.Handle);

            return builder.Build();
        }
    }
}