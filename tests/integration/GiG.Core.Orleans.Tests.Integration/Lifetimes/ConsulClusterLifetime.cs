using Bogus;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Abstractions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class ConsulClusterLifetime : IAsyncLifetime
    {
        internal  IOptions<ConsulOptions> ConsulOptions;

        internal  IHttpClientFactory HttpClientFactory;

        internal  IClusterClient ClusterClient;

        internal  IServiceProvider ClientServiceProvider;

        internal  string SiloName;

        internal string ConsulKvStoreBaseAddress;

        private IHost _siloHost;

        public async Task InitializeAsync()
        {
            SiloName = new Faker().Random.String2(5);

            _siloHost = Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureCluster(ctx.Configuration);
                    sb.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:ConsulSilo"));
                    sb.ConfigureConsulClustering(ctx.Configuration);
                    sb.AddAssemblies(typeof(EchoTestGrain));
                    sb.Configure<SiloOptions>(siloOptions => siloOptions.SiloName = SiloName);
                })
                .Build();

            await _siloHost.StartAsync();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<ConsulOptions>(ctx.Configuration);

                    services.AddHttpClient();
                    services.AddDefaultClusterClient(x =>
                    {
                        x.ConfigureCluster(ctx.Configuration);
                        x.ConfigureConsulClustering(ctx.Configuration);
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            HttpClientFactory = ClientServiceProvider.GetRequiredService<IHttpClientFactory>();

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();

            ConsulOptions = ClientServiceProvider.GetRequiredService<IOptions<ConsulOptions>>();

            var options = ConsulOptions.Value;
            ConsulKvStoreBaseAddress = $"{options.Address}/v1/kv/{options.KvRootFolder}/";
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
