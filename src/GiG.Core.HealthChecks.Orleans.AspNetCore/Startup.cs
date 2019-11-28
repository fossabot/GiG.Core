using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.HealthChecks.Orleans.AspNetCore
{
    internal class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureHealthChecks(_configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks();
        }
    }
}