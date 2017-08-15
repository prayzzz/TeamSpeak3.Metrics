using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Startup : StartupBase
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                    .AddJsonFile("appsettings.json", false, true)
                                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                                                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public override void Configure(IApplicationBuilder app)
        {
            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("api/metrics", context =>
            {
                var controller = context.RequestServices.GetService<TeamSpeakData>();
                return context.Response.WriteAsync(controller.Get());
            });

            app.UseRouter(routeBuilder.Build());
        }

        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("App"));

            services.AddRouting();

            services.AddSingleton<TeamSpeakData>();
            services.AddSingleton<IHostedService, DataRefresher>();
            services.AddSingleton(provider => new Func<TeamSpeakConnection>(() => new TeamSpeakConnection(provider.GetService<ILogger<TeamSpeakConnection>>())));

            return services.BuildServiceProvider();
        }
    }
}