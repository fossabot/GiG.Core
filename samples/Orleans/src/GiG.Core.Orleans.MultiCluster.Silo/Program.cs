using GiG.Core.Configuration.Extensions;
using Microsoft.Extensions.Hosting;
using GiG.Core.Logging.All.Extensions;

namespace GiG.Core.Orleans.MultiCluster.Silo
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
                .ConfigureServices(Startup.ConfigureServices)
                .UseOrleans(Startup.ConfigureOrleans);
    }    
}
