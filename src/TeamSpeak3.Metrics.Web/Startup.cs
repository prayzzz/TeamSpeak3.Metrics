using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.AspNetCore;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Web.Common;

namespace TeamSpeak3.Metrics.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStartupInfoLogging(_logger);
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServerOptions>(_configuration.GetSection("App:TS3Server"));
            services.AddTeamSpeak3Metrics().AsHostedService();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}
