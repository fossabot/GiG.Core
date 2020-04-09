using System.Text.Json.Serialization;
using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.Authentication.ApiKey.Extensions;
using GiG.Core.Web.Docs.Authentication.ApiKey.Extensions;
using GiG.Core.Web.Docs.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddActivityAccessor();
            services.AddActivityTenantAccessor();
            
            services.ConfigureApiDocs(Configuration)
                .AddApiDocsApiKeyAuthentication()
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            
            services.AddApiKeyAuthentication();
            services.ConfigureApiKeyOptions(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseTenantIdMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiDocs();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}