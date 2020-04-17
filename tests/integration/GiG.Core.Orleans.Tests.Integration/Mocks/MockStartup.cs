using Bogus;
using GiG.Core.HealthChecks.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Orleans.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockStartup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseHealthChecks();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public static void ConfigureOrleansServices(IServiceCollection services)
        {
            services.AddHealthChecks().AddOrleansHealthCheck();
        }

        // This method gets called by the runtime. Use this method to configure Orleans.
        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            var serviceId = new Randomizer().String2(8);
            var clusterId = new Randomizer().String2(8);
            var options = ctx.Configuration.GetSection("Orleans:HealthCheckCoHostedSilo").Get<EndpointOptions>() ?? new EndpointOptions();

            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:HealthCheckCoHostedSilo"))
                .UseLocalhostClustering(options.SiloPort, options.GatewayPort, null, serviceId, clusterId)
                .AddHealthCheckDependencies();
        }
    }
}
