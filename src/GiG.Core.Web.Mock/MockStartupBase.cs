using GiG.Core.Validation.FluentValidation.Web.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using GiG.Core.Web.Mock.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Mock
{
    /// <summary>
    /// Mock Startup base class.
    /// </summary>
    public abstract class MockStartupBase
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureForwardedHeaders();
            services.ConfigureApiBehaviorOptions();
            services.AddRouting();

            services.AddMockRequestContextAccessor();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseRouting();
            app.UseFluentValidationMiddleware();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
