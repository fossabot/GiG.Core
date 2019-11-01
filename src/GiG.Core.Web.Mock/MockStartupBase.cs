using FluentValidation.AspNetCore;
using GiG.Core.Context.Web.Extensions;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Mock
{
    public abstract class MockStartupBase
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();
            services.ConfigureForwardedHeaders();
            services.ConfigureApiBehaviorOptions();
            services.AddRouting();
            
            services.AddRequestContextAccessor();
            services.AddCorrelationAccessor();
            services.AddTenantAccessor();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseCorrelation();
            app.UseRouting();
            app.UseFluentValidationMiddleware();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
