using FluentValidation.AspNetCore;
using GiG.Core.Web.FluentValidation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.FluentValidation.Tests.Integration.Mocks
{
    internal class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation();
            
            // Configure Api Behavior Options
            services.ConfigureApiBehaviorOptions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseFluentValidationMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
