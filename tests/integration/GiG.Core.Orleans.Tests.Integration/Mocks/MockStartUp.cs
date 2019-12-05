using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Orleans.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockStartUp
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
        public static void ConfigureOrleans(Microsoft.Extensions.Hosting.HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureEndpoints(ctx.Configuration)
                .UseLocalhostClustering(11115, 30005)
                .AddHealthCheckDependencies();
        }
    }
}
