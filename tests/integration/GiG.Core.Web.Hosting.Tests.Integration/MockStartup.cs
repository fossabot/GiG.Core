using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Hosting.Tests.Integration
{
    internal class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureForwardedHeaders();
        }

        public void Configure(IApplicationBuilder app)
        { 
            app.UseRouting();
            app.ConfigurePathBase();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
