using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Logging.Sample
{
    internal static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseApplicationMetadata()
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
        }
    }
}