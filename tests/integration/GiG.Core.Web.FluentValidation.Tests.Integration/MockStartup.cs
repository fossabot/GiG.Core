using FluentValidation.AspNetCore;
using GiG.Core.Web.FluentValidation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.FluentValidation.Tests.Integration
{
    internal class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();
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
