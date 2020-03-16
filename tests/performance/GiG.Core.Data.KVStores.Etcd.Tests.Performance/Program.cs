using GiG.Core.Configuration.Extensions;
using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Data.KVStores.Etcd.Tests.Performance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseApplicationMetadata()
                .ConfigureServices(x => x.AddActivityAccessor())
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}