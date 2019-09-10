using FluentValidation.AspNetCore;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Web.FluentValidation.Extensions;
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
            
            // Services
            services.AddSingleton<ITransactionService, TransactionService>();
	        services.AddCachedHealthChecks(_configuration)
                .AddCachedCheck<DummyCachedHealthCheck>(nameof(DummyCachedHealthCheck));

            // WebAPI       
            services.AddApplicationMetadataAccessor();
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCorrelationId();
            app.UseRouting();
            app.UseFluentValidationMiddleware();
            app.UseHealthChecks();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
