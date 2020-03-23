using Microsoft.Extensions.Hosting;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run(); 
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(Startup.ConfigureServices);
    }
}