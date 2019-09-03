using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Extensions.HealthCheck.Tests.Integration
{
    public class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCachedHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks();
        }
       
    }
}
