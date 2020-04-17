using Bogus;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Silo.Metrics.Prometheus.Extensions;
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
            var options = ctx.Configuration.GetSection("Orleans:MetricsSilo").Get<EndpointOptions>() ?? new EndpointOptions();

            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:MetricsSilo"))
                .UseLocalhostClustering(options.SiloPort, options.GatewayPort, null, serviceId, clusterId)
                .AddPrometheusTelemetry(ctx.Configuration);
        }
    }
}
