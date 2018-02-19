using App.Metrics.AspNetCore;
using App.Metrics.Health;
using App.Metrics.AspNetCore.Health;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace scorednameAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseMetrics()
                .UseHealth()
                .UseStartup<Startup>()
                .Build();
    }
}
