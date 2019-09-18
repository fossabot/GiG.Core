using GiG.Core.Context.Web.Extensions;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Orleans.Sample.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(x =>
                    {
                        x.AddCorrelationId();
                        x.AddRequestContext();
                    })
                .UseApplicationMetadata()
                .ConfigureLogging()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}