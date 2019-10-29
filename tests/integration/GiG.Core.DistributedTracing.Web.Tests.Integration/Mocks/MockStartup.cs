using GiG.Core.DistributedTracing.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.DistributedTracing.Web.Tests.Integration.Mocks
{
    internal class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationAccessor();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCorrelationId();
        }
    }
}
