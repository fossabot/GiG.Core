using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.MultiTenant.Web.Internal;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.Mock;
using GiG.Core.Web.Mock.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Mocks
{
    internal class MockStartup : MockStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivityAccessor();
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
