using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    internal class MockStartup : MockStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationAccessor();
            services.AddTenantAccessor();
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseCorrelation();
            base.Configure(app);
        }
    }
}