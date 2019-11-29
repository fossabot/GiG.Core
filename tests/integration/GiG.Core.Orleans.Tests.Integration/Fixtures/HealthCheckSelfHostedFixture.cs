using GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Orleans.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;
using System.Net.Http;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class HealthCheckSelfHostedFixture : IDisposable
    {
        internal readonly IHttpClientFactory HttpClientFactory;

        private readonly IHost _host;

        public HealthCheckSelfHostedFixture()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .UseOrleans((ctx, x) =>
                {
                    x.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:ClusterB:Silo"));
                    x.UseLocalhostClustering(11116, 30006);
                    x.AddHealthCheckDependencies();
                })
                .ConfigureServices((ctx, x) =>
                {
                    x.AddHttpClient();
                    x.AddHealthChecks().AddOrleansHealthCheck();
                    x.AddHealthChecksService(ctx.Configuration);
                })
                .Build();

            _host.StartAsync().GetAwaiter().GetResult();

            HttpClientFactory = _host.Services.GetService<IHttpClientFactory>();
        }

        public void Dispose()
        {
            _host?.StopAsync();
            _host?.WaitForShutdown();
        }
    }
}