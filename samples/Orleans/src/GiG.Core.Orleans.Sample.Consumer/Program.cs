using GiG.Core.Configuration.Extensions;
using Microsoft.Extensions.Hosting;
using GiG.Core.Logging.All.Extensions;

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