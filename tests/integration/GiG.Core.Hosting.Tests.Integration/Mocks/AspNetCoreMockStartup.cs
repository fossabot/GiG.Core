using GiG.Core.Hosting.AspNetCore.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Web.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Hosting.Tests.Integration.Mocks
{
    internal class AspNetCoreMockStartup : MockStartupBase
    {
        private readonly IConfiguration _configuration;

        public AspNetCoreMockStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureInfoManagement(_configuration);
            base.ConfigureServices(services);
        }

        /// <inheritdoc />
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            app.UseEndpoints(endpoints => { endpoints.MapInfoManagement(); });
        }
    }
}