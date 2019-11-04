using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.MultiTenant.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    internal class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting();
            services.AddCorrelationAccessor();
            services.AddTenantAccessor();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCorrelation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}