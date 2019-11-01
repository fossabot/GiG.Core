using GiG.Core.Http.Security.Hmac;
using GiG.Core.Security.Http;
using GiG.Core.Web.Security.Hmac.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Security.Hmac.Tests.Integration.Mocks
{
    internal class MockStartup
    {
        private readonly IConfiguration _configuration;

        public MockStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHmacAuthentication();
            services.ConfigureDefaultHmacOptionProvider(_configuration.GetSection("Hmac"));
            services.AddSingleton<HmacDelegatingHandler>();
            services.AddSingleton<IHmacOptionsProvider, DefaultHmacOptionsProvider>();
            services.AddSingleton<IHmacSignatureProvider, HmacSignatureProvider>();
            services.Configure<HmacOptions>(_configuration.GetSection("Hmac"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
