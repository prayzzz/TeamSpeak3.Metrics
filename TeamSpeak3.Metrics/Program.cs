using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TeamSpeak3.Metrics.Common;

namespace TeamSpeak3.Metrics
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder().UseKestrel()
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
            var configuration = context.Configuration.GetSection("Logging").Get<LoggingConfiguration>();

            // Enable all logs
            builder.SetMinimumLevel(LogLevel.Trace);

            Logger logger;
            if (!context.HostingEnvironment.IsProduction())
            {
                logger = new LoggerConfiguration().MinimumLevel.Debug()
                                                  .Enrich.FromLogContext()
                                                  .WriteTo.LiterateConsole(LogEventLevel.Information)
                                                  .WriteTo.Trace(LogEventLevel.Debug)
                                                  .CreateLogger();
            }
            else
            {
                logger = new LoggerConfiguration().MinimumLevel.Information()
                                                  .Enrich.FromLogContext()
                                                  .WriteTo.LiterateConsole()
                                                  .WriteTo.RollingFile(configuration.PathFormat, retainedFileCountLimit: 7)
                                                  .CreateLogger();
            }
            
            //builder.AddFilter((category, level) =>
            //{
            //    // hide debug logs from framework
            //    if (level == LogLevel.Debug &&
            //        (category.StartsWith("Microsoft", StringComparison.CurrentCultureIgnoreCase) ||
            //        category.StartsWith("System", StringComparison.CurrentCultureIgnoreCase)))
            //    {
            //        return false;
            //    }

            //    return true;
            //});

            builder.AddProvider(new SerilogLoggerProvider(logger));
        }
    }
}