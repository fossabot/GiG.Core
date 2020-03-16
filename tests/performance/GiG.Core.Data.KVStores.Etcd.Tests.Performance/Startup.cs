using FluentValidation.AspNetCore;
using GiG.Core.Data.KVStores.Etcd.Tests.Performance.Models;
using GiG.Core.Data.KVStores.Etcd.Tests.Performance.Services;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.HealthChecks.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Hosting.AspNetCore.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.FluentValidation.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace GiG.Core.Data.KVStores.Etcd.Tests.Performance
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Info Management
            services.ConfigureInfoManagement(Configuration);

            // Health Checks
            services.ConfigureHealthChecks(Configuration)
                .AddHealthChecks();

            // Web Api
            services.ConfigureApiDocs(Configuration)
                .AddControllers()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());

            // Forwarded Headers
            services.ConfigureForwardedHeaders();

            // Configure Api Behavior Options
            services.ConfigureApiBehaviorOptions();
            
            services.AddActivityAccessor();

            services.AddHostedService<CurrencyService>();
            
            services.AddKVStores<IEnumerable<Currency>>()
                .FromEtcd(Configuration, "Currencies")
                .WithJsonSerialization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseRouting();
            app.UseFluentValidationMiddleware();
            app.UseHealthChecks();
            app.UseInfoManagement();
            app.UseApiDocs();
            
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
                endpoints.MapInfoManagement();
                endpoints.MapHealthChecks();
            });            
        }
    }
}