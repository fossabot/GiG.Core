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

        private IHost SiloHost;

        internal IServiceProvider ClientServiceProvider;

        internal string SiloName;

        public async Task InitializeAsync()
        {
            SiloName = new Faker().Random.String2(5);

            SiloHost = Host.CreateDefaultBuilder()
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

            await SiloHost.StartAsync();

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

            ClientServiceProvider = clientHost.Services;

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();

            KubernetesOptions = SiloHost.Services.GetRequiredService<IOptions<KubernetesSiloOptions>>();
        }

        public async Task DisposeAsync()
        {
            await SiloHost?.StopAsync();
            await ClusterClient?.Close();
        }
    }
}