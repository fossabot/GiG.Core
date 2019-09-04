using GiG.Core.Extensions.Configuration;
using GiG.Core.Extensions.DistributedTracing.Web;
using GiG.Core.Extensions.Logging.All;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Web.Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(x => x.AddCorrelationId())
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}