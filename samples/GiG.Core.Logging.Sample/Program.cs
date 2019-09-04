using System.IO;
using GiG.Core.Extensions.Logging.All;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Logging.Sample
{
    static class Program
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
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
        }
    }
}