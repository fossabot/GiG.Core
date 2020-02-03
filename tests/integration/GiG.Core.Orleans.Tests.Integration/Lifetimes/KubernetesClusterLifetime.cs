using Bogus;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Abstractions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
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
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class KubernetesClusterLifetime : IAsyncLifetime
    {
        internal IOptions<KubernetesSiloOptions> KubernetesOptions;

        internal IClusterClient ClusterClient;

        private IHost _siloHost;

        private IServiceProvider _clientServiceProvider;

        internal string SiloName;

        public async Task InitializeAsync()
        {
            SiloName = new Faker().Random.String2(5);

            _siloHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<KubernetesSiloOptions>(ctx.Configuration.GetSection("Orleans:KubernetesMembershipProvider"));
                })
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureCluster(ctx.Configuration);
                    sb.ConfigureEndpoints(ctx.Configuration);
                    sb.ConfigureKubernetesClustering(ctx.Configuration.GetSection("Orleans:KubernetesMembershipProvider"));
                    sb.AddAssemblies(typeof(EchoTestGrain));
                    sb.Configure<SiloOptions>(options => options.SiloName = SiloName);
                })
                .Build();

            await _siloHost.StartAsync();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHttpClient();
                    services.AddDefaultClusterClient(x =>
                    {
                        x.ConfigureCluster(ctx.Configuration);
                        x.ConfigureKubernetesClustering(ctx.Configuration.GetSection("Orleans:KubernetesMembershipProvider"));
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            _clientServiceProvider = clientHost.Services;

            ClusterClient = _clientServiceProvider.GetRequiredService<IClusterClient>();

            KubernetesOptions = _siloHost.Services.GetRequiredService<IOptions<KubernetesSiloOptions>>();
        }

        public async Task DisposeAsync()
        {
            if (_siloHost != null)
            {
                await _siloHost?.StopAsync();
            }

            if (ClusterClient != null)
            {
                await ClusterClient?.Close();
            }
        }
    }
}