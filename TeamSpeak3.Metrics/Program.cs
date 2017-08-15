using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TeamSpeak3.Metrics
{
    public static class Program
    {
        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
    }
}