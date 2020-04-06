using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using GiG.Core.Logging.All.Extensions;

namespace GiG.Core.Performance.Logging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}