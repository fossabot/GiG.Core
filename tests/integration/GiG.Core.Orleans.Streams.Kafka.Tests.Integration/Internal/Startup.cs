using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Orleans.Streams.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Internal
{
    internal class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureHealthChecks(Configuration);

            services.AddCachedHealthChecks()
                .AddKafkaOrleansStreams(Configuration, tags: new [] { HealthChecks.Abstractions.Constants.ReadyTag });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks();
        }
    }
}