using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Startup : StartupBase
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public override void Configure(IApplicationBuilder app)
        {
            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("api/metrics", context =>
            {
                var controller = context.RequestServices.GetService<ITeamSpeakMetrics>();
                var metrics = controller.Metrics;

                return context.Response.WriteAsync(JsonConvert.SerializeObject(metrics));
            });

            app.UseRouter(routeBuilder.Build());
        }

        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            base.CreateServiceProvider(services);

            services.Configure<AppConfiguration>(Configuration.GetSection("App"));
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
    }
}