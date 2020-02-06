using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Web.Mock;
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
    }
}
