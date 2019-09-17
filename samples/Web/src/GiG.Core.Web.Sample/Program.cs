using GiG.Core.Configuration.Extensions;
using GiG.Core.Context.Web.Extensions;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
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
                .UseApplicationMetadata()
                .ConfigureServices(x => {
                    x.AddCorrelationId();
                    x.AddRequestContext();
                })
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}