using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TeamSpeak3.Metrics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder().UseKestrel()
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<Startup>()
                                .Build()
                                .Run();
        }
    }
}