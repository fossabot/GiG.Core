using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

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