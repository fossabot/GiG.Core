using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.Authentication.ApiKey.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Mocks
{
    internal class MockStartup
    {
        private readonly IConfiguration _configuration;

        public MockStartup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddActivityTenantAccessor();
            services.AddApiKeyAuthentication();
            services.ConfigureApiKeyOptions(_configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UseRouting();
            app.UseTenantIdMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
