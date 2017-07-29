using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics
{
    public class Startup : StartupBase
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public override void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<PeriodicDataCollector>();
            services.AddSingleton(async provider =>
            {
                var query = new TeamSpeakQuery(provider.GetService<ILogger<TeamSpeakQuery>>());
                query.Connect("192.168.1.10", 10011);
                await query.Login("admin", "LLwgvOk9");
                await query.Use(9987);

                return query;
            });

            return services.BuildServiceProvider();
        }
    }
}