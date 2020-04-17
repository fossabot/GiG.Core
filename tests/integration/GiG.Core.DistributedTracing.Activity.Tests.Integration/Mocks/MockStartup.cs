using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.Web.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Mocks
{
    internal class MockStartup : MockStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivityContextAccessor();
            services.AddHttpClient();
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseTenantIdMiddleware();
            
            base.Configure(app);
        }
    }
}
