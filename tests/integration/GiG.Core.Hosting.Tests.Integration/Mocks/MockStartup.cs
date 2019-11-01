using GiG.Core.Hosting.Extensions;
using GiG.Core.Web.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Hosting.Tests.Integration.Mocks
{
    internal class MockStartup : MockStartupBase
    {
        private readonly IConfiguration _configuration;

        public MockStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.ConfigureInfoManagement(_configuration);
        }

        /// <inheritdoc />
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            app.UseInfoManagement();
        }
    }
}