using Bogus;
using GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Orleans.Extensions;
using GiG.Core.Orleans.Silo.Abstractions;
using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class HealthCheckSelfHostedLifetime : IAsyncLifetime
    {
        internal IHttpClientFactory HttpClientFactory;

        private IHost _host;
       
        public async Task InitializeAsync()
        {
            var serviceId = new Randomizer().String2(8);
            var clusterId = new Randomizer().String2(8);

            _host = new HostBuilder()
              .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
              .UseOrleans((ctx, x) =>
              {
                  var options = ctx.Configuration.GetSection("Orleans:HealthChecksSelfHostedSilo").Get<SiloOptions>() ?? new SiloOptions();
                  x.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:HealthChecksSelfHostedSilo"));
                  x.UseLocalhostClustering(options.SiloPort, options.GatewayPort, null, serviceId, clusterId);
                  x.AddHealthCheckDependencies();
              })
              .ConfigureServices((ctx, x) =>
              {
                  x.AddHttpClient();
                  x.AddHealthChecks().AddOrleansHealthCheck();
                  x.AddOrleansHealthChecksSelfHosted(ctx.Configuration.GetSection("Orleans:HealthChecksSelfHostedSilo:HostSelf"));
              })
              .Build();

            await _host.StartAsync();

            HttpClientFactory = _host.Services.GetService<IHttpClientFactory>();
        }

        public async Task DisposeAsync()
        {
            await _host?.StopAsync();
            await _host?.WaitForShutdownAsync();
            _host.Dispose();
        }

    }
}