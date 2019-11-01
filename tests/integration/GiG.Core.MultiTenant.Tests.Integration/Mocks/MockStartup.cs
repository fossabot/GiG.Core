using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.Mock;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.MultiTenant.Web.Tests.Integration.Mocks
{
    internal class MockStartup : MockStartupBase
    {
        /// <inheritdoc />
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTenantAccessor();
            base.ConfigureServices(services);
        }
    }
}
