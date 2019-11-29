using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Hosting.AspNetCore.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Messaging.Kafka.Producer.Sample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.Configure<KafkaProviderOptions>(_configuration.GetSection(KafkaProviderOptions.DefaultSectionName));
            services.ConfigureInfoManagement(_configuration);

            services.AddCorrelationAccessor();
            
            // WebAPI
            services
                .ConfigureApiDocs(_configuration)
                .AddControllers();
            
            // Forwarded Headers
            services.ConfigureForwardedHeaders();

            // Configure Api Behavior Options
            services.ConfigureApiBehaviorOptions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseCorrelation();
            app.UseRouting();
            app.UseApiDocs();
            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapInfoManagement();
            });
        }
    }
}