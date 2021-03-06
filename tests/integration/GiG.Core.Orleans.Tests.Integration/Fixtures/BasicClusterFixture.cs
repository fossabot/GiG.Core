using Bogus;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Localhost.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Streams.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class BasicClusterFixture : IAsyncLifetime
    {
        private const string SiloSectionName = "Orleans:BasicClusterSilo";

        internal IClusterClient ClusterClient;

        internal IServiceProvider ClientServiceProvider;

        private IHost _siloHost;
        
        public async Task InitializeAsync()
        {
            var serviceId = new Randomizer().String2(8);
            var clusterId = new Randomizer().String2(8);

            _siloHost = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .UseOrleans((ctx, x) =>
                {
                    var endpointOptions = ctx.Configuration.GetSection(SiloSectionName).Get<EndpointOptions>() ?? new EndpointOptions();
                    x.ConfigureEndpoints(ctx.Configuration.GetSection(SiloSectionName));
                    x.UseMembershipProvider(ctx.Configuration, y =>
                    {
                        y.ConfigureLocalhostClustering(endpointOptions.SiloPort, endpointOptions.GatewayPort, null, serviceId, clusterId);
                    });
                    x.AddAssemblies(typeof(CommandTestGrain));
                    x.AddSimpleMessageStreamProvider(Constants.StreamProviderName);
                    x.AddMemoryGrainStorage(Constants.StreamsMemoryStorageName);
                })
                .ConfigureServices(x =>
                {
                    x.AddStream();
                })
                .Build();
            await _siloHost.StartAsync();

            var config = _siloHost.Services.GetService<IConfiguration>();
            var options = config.GetSection(SiloSectionName).Get<EndpointOptions>() ?? new EndpointOptions();

            var clientHost = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddStream();
                    services.AddCommandDispatcher();
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.UseMembershipProvider(config, y => { y.ConfigureLocalhostClustering(options.GatewayPort, serviceId, clusterId); });
                        x.AddAssemblies(typeof(ICommandTestGrain));
                        x.AddSimpleMessageStreamProvider(Constants.StreamProviderName);
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();
        }

        public async Task DisposeAsync()
        {
            if (ClusterClient != null)
            {
                await ClusterClient.Close();
            }

            if (_siloHost != null)
            {
                await _siloHost.StopAsync();
            }
        }
    }
}