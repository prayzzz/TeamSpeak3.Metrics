using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TeamSpeak3.Metrics
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var hostConfig = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            new WebHostBuilder().UseKestrel()
                                .UseConfiguration(hostConfig)
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .ConfigureAppConfiguration(ConfigureConfiguration)
                                .ConfigureLogging(ConfigureLogging)
                                .UseStartup<Startup>()
                                .Build()
                                .Run();
        }

        private static void ConfigureConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            var env = context.HostingEnvironment;
            builder.AddJsonFile("appsettings.json", true, true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName.ToLower()}.json", true, true);
            builder.AddEnvironmentVariables();
        }

        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder builder)
        {
            // Enable all logs
            builder.SetMinimumLevel(LogLevel.Trace);

            var logger = new LoggerConfiguration().ReadFrom
                                                  .Configuration(context.Configuration)
                                                  .CreateLogger();

            builder.AddSerilog(logger);
        }
    }
}