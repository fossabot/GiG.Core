using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Logging.Sample
{
    internal static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<HelloWorldService>();
        }
    }
}