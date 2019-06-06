using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TeamSpeak3.Metrics.Web.Common
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseStartupInfoLogging(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var applicationLifetime = scope.ServiceProvider.GetService<IApplicationLifetime>();
                applicationLifetime.ApplicationStarted.Register(() =>
                {
                    var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
                    logger.LogInformation("Hosting environment: {EnvironmentName}", hostingEnvironment.EnvironmentName);
                    logger.LogInformation("Content root path: {ContentRootPath}", hostingEnvironment.ContentRootPath);

                    var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
                    if (serverAddressesFeature != null)
                    {
                        logger.LogInformation("Now listening on: {Url}", string.Join(", ", serverAddressesFeature.Addresses));
                    }
                });
            }
        }
    }
}
