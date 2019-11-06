using GiG.Core.Context.Web.Extensions;
using GiG.Core.Web.Mock;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Context.Web.Tests.Integration.Mocks
{
    internal class MockStartup : MockStartupBase
    {
        /// <inheritdoc />
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddRequestContextAccessor();
            base.ConfigureServices(services);
        }
    }
}
