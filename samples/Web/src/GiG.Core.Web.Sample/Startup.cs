using GiG.Core.HealthChecks.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Logging.AspNetCore.Extensions;
using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.Web.Authentication.ApiKey.Extensions;
using GiG.Core.Web.Authentication.OAuth.Extensions;
using GiG.Core.Web.Docs.Authentication.ApiKey.Extensions;
using GiG.Core.Web.Docs.Authentication.OAuth.Extensions;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.Versioning.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace GiG.Core.Web.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddApiExplorerVersioning();
            
            services.ConfigureApiDocs(Configuration)
                .AddApiDocsApiKeyAuthentication()
                .AddApiDocsOAuthAuthentication();
            
            services.ConfigureApiKeyOptions(Configuration)
                .AddApiKeyAuthentication();

            services.ConfigureOAuthAuthentication(Configuration);

            services.ConfigureHttpRequestResponseLogging(Configuration);

            services.ConfigureHealthChecks(Configuration)
                .AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseTenantIdMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiDocs();
            app.UseHttpRequestResponseLogging();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks();
            });
        }
    }
}