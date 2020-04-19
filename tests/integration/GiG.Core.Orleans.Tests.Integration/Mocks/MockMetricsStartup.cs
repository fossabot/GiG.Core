using Bogus;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Metrics.Prometheus.Orleans.Silo.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using Prometheus;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockMetricsStartup
    {
        private const string MetricsSilo = "Orleans:MetricsSilo";
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpMetrics();
            app.UseMetricServer();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure Orleans.
        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            var serviceId = new Randomizer().String2(8);
            var clusterId = new Randomizer().String2(8);
            var options = ctx.Configuration.GetSection(MetricsSilo).Get<EndpointOptions>() ?? new EndpointOptions();

            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureEndpoints(ctx.Configuration.GetSection(MetricsSilo))
                .UseLocalhostClustering(options.SiloPort, options.GatewayPort, null, serviceId, clusterId)
                .AddPrometheusTelemetry(ctx.Configuration);
        }
    }
}
