using Bogus;
using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Localhost.Extensions;
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

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ActivityClusterFixture : IAsyncLifetime
    {
        private const string SiloSectionName = "Orleans:ActivitySilo";

        internal IClusterClient ClusterClient;

        internal IServiceProvider ClientServiceProvider;

        private IHost _siloHost;

        public async Task InitializeAsync()
        {
            var serviceId = new Randomizer().String2(8);
            var clusterId = new Randomizer().String2(8);

            _siloHost = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .ConfigureServices(x =>
                 {
                     x.AddActivityTenantAccessor();
                     x.AddActivityAccessor();
                     x.AddRequestContextAccessor();
                     x.AddStream();
                 })
                .UseOrleans((ctx, x) =>
                {
                    var siloOptions = ctx.Configuration.GetSection(SiloSectionName).Get<SiloOptions>() ?? new SiloOptions();
                    x.AddActivityIncomingFilter();
                    x.ConfigureEndpoints(ctx.Configuration.GetSection(SiloSectionName));
                    x.UseMembershipProvider(ctx.Configuration, y =>
                    {
                        y.ConfigureLocalhostClustering(siloOptions.SiloPort, siloOptions.GatewayPort, null, serviceId, clusterId);
                    });
                    x.AddAssemblies(typeof(EchoTestGrain));
                    x.AddSimpleMessageStreamProvider(Constants.StreamProviderName);
                    x.AddMemoryGrainStorage(Constants.StreamsMemoryStorageName);
                    x.AddMemoryGrainStorage(Constants.StorageProviderName);
                })
                .Build();
            await _siloHost.StartAsync();

            var config = _siloHost.Services.GetService<IConfiguration>();
            var options = config.GetSection(SiloSectionName).Get<SiloOptions>() ?? new SiloOptions();

            var clientHost = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddActivityAccessor();
                    services.AddStream();
                    services.AddSingleton<IRequestContextAccessor, MockRequestContextAccessor>();
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.AddActivityOutgoingFilter(sp);
                        x.AddRequestContextOutgoingFilter(sp);
                        x.UseMembershipProvider(config, y => { y.ConfigureLocalhostClustering(options.GatewayPort, serviceId, clusterId); });
                        x.AddAssemblies(typeof(IEchoTestGrain));
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