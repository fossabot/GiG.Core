using GiG.Core.ApplicationMetrics.Extensions;
using GiG.Core.ApplicationMetrics.Prometheus.Extensions;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using GiG.Core.Web.Mock.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.ApplicationMetrics.Prometheus.Tests.Integration.Mocks
{
    public class MockStartup
    {
        private readonly IConfiguration _configuration; 

        public MockStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureForwardedHeaders();
            services.ConfigureApiBehaviorOptions();
            services.AddRouting();
            services.ConfigureApplicationMetrics(_configuration);

            services.AddMockRequestContextAccessor();
            services.AddMockCorrelationAccessor();
            services.AddMockTenantAccessor();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseRouting();
            app.UseHttpApplicationMetrics();
            app.UseFluentValidationMiddleware();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapApplicationMetrics();
            });
        }
    }
}