using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Messaging.Kafka.Sample
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
    }
}