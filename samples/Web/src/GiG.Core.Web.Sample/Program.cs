using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.MultiTenant.Activity.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Web.Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(x =>
                {
                    x.AddActivityContextAccessor();
                    x.AddActivityTenantAccessor();
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}