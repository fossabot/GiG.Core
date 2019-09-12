using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Data.Migration.Sample
{
    static class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
        }
    }
}
