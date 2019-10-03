using GiG.Core.Configuration.Extensions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Orleans.Sample.Silo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseApplicationMetadata()
                .ConfigureServices(services => {
                    services.AddCorrelationAccessor();
                    services.AddRequestContextAccessor();
                })
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices)
                .UseOrleans(Startup.ConfigureOrleans);
    }
}