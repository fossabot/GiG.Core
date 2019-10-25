using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace GiG.Core.Data.KVStores.Sample
{
    internal class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(builder => builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true))
                .UseApplicationMetadata()
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
        }
    }
}