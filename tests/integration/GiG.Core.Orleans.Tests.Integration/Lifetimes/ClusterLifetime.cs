using Bogus;
using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Orleans;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Silo.Abstractions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Streams.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public abstract class ClusterLifetime : IAsyncLifetime
    {
        private readonly string _siloSectionName;
        
        internal IClusterClient ClusterClient;

        internal IServiceProvider ClientServiceProvider;

        private IHost _siloHost;

        protected ClusterLifetime(string siloSectionName) => _siloSectionName = siloSectionName;

        public async Task InitializeAsync()
        {
            var serviceId = new Randomizer().String2(8);
            var clusterId = new Randomizer().String2(8);

            _siloHost = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .UseOrleans((ctx, x) =>
                {
                    var siloOptions = ctx.Configuration.GetSection(_siloSectionName).Get<SiloOptions>() ?? new SiloOptions();

                    x.ConfigureEndpoints(ctx.Configuration.GetSection(_siloSectionName));
                    x.UseLocalhostClustering(siloOptions.SiloPort, siloOptions.GatewayPort, null, serviceId, clusterId);
                    x.AddAssemblies(typeof(EchoTestGrain));
                    x.AddSimpleMessageStreamProvider("SMSProvider");
                    x.AddMemoryGrainStorage("PubSubStore");
                })
                .ConfigureServices(x =>
                {
                    x.AddCorrelationAccessor();
                    x.AddRequestContextAccessor();
                    x.AddStream();
                })
                .Build();
            await _siloHost.StartAsync();

            var config = _siloHost.Services.GetService<IConfiguration>();
            var options = config.GetSection(_siloSectionName).Get<SiloOptions>() ?? new SiloOptions();

            var clientHost = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
                    services.AddSingleton<IRequestContextAccessor, MockRequestContextAccessor>();
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.AddCorrelationOutgoingFilter(sp);
                        x.AddRequestContextOutgoingFilter(sp);
                        x.UseLocalhostClustering(options.GatewayPort, serviceId, clusterId);
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();
        }

        public async Task DisposeAsync()
        {
            await ClusterClient?.Close();
            await _siloHost?.StopAsync();
        }
    }
}