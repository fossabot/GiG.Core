using GiG.Core.Configuration.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
    }
}