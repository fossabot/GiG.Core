using GiG.Core.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Logging.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] arg)
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(configHost => configHost.AddJsonFile("appsettings.json"))
                .UseLogging()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddHostedService<HelloWorld>();
                });
        }
    }
}
