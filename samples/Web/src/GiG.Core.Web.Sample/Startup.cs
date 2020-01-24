using FluentValidation.AspNetCore;
using GiG.Core.ApplicationMetrics;
using GiG.Core.ApplicationMetrics.Prometheus;
using GiG.Core.HealthChecks.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Hosting.AspNetCore.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Web.Authentication.OAuth.Extensions;
using GiG.Core.Web.Docs.Authentication.OAuth.Extentions;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using GiG.Core.Web.Sample.Contracts;
using GiG.Core.Web.Sample.HealthChecks;
using GiG.Core.Web.Sample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Sample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.Configure<TransactionSettings>(_configuration.GetSection(TransactionSettings.DefaultSectionName));
            services.ConfigureInfoManagement(_configuration);

            // Services
            services.AddSingleton<ITransactionService, TransactionService>();

            // Health Checks
            services
                .ConfigureHealthChecks(_configuration)
                .AddCachedHealthChecks()
                .AddReadyCheck<DummyCachedHealthCheck>(nameof(DummyCachedHealthCheck));

            // WebAPI
            services
                .ConfigureApiDocs(_configuration)
                .AddControllers()
                   .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());

            // Forwarded Headers
            services.ConfigureForwardedHeaders();

            // Configure Api Behavior Options
            services.ConfigureApiBehaviorOptions();

            //Authentication
            services.ConfigureOAuthAuthentication(_configuration)
                .AddApiDocsOAuthAuthentication();
            
            //Application Metrics
            services
                .ConfigureApplicationMetrics(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseRouting();
            app.UseFluentValidationMiddleware();
            app.UseApiDocs();
            app.UseHttpApplicationMetrics();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapInfoManagement();
                endpoints.MapHealthChecks();
                endpoints.MapApplicationMetrics();
            });
        }
    }
}