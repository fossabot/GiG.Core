using Bogus;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Abstractions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Abstractions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.Configuration;
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
    public abstract class MembershipProviderLifetime : IAsyncLifetime
    {
        private readonly string _membershipProviderSectionName;

        private readonly string _clusterOptionsSectionName;

        private readonly string _siloOptionsSectionName;

        private IOptions<ConsulOptions> _consulOptions;

        internal IOptions<KubernetesSiloOptions> KubernetesOptions;

        internal IClusterClient ClusterClient;

        internal IHttpClientFactory HttpClientFactory;

        private IServiceProvider _clientServiceProvider;

        internal string ConsulKvStoreBaseAddress;

        private IHost _siloHost;

        internal string SiloName;

        public MembershipProviderLifetime(string membershipProviderSectionName, string clusterOptionsSectionName, string siloOptionsSectionName)
        {
            _membershipProviderSectionName = membershipProviderSectionName;
            _clusterOptionsSectionName = clusterOptionsSectionName;
            _siloOptionsSectionName = siloOptionsSectionName;
        }

        public async Task InitializeAsync()
        {
            SiloName = new Faker().Random.String2(5);

            _siloHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .ConfigureServices((ctx, services) =>
                {
                    var membershipProviderSection = ctx.Configuration.GetSection(_membershipProviderSectionName);
                    services.Configure<KubernetesSiloOptions>(membershipProviderSection);
                })
                .UseOrleans((ctx, sb) =>
                {
                    var membershipProviderSection = ctx.Configuration.GetSection(_membershipProviderSectionName);

                    sb.ConfigureCluster(ctx.Configuration.GetSection(_clusterOptionsSectionName));
                    sb.ConfigureEndpoints(ctx.Configuration.GetSection(_siloOptionsSectionName));
                    sb.UseMembershipProvider(membershipProviderSection, x =>
                    {
                        x.ConfigureConsulClustering(membershipProviderSection);
                        x.ConfigureKubernetesClustering(membershipProviderSection);
                    });
                    sb.AddAssemblies(typeof(EchoTestGrain));
                    sb.Configure<SiloOptions>(siloOptions => siloOptions.SiloName = SiloName);
                })
                .Build();

            await _siloHost.StartAsync();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    var membershipProviderSection = ctx.Configuration.GetSection(_membershipProviderSectionName);

                    services.AddHttpClient();
                    services.AddDefaultClusterClient(x =>
                    {
                        x.ConfigureCluster(ctx.Configuration.GetSection(_clusterOptionsSectionName));
                        x.UseMembershipProvider(membershipProviderSection, builder =>
                        {
                            builder.ConfigureConsulClustering(membershipProviderSection);
                            builder.ConfigureKubernetesClustering(membershipProviderSection);
                        });
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            _clientServiceProvider = clientHost.Services;

            HttpClientFactory = _clientServiceProvider.GetRequiredService<IHttpClientFactory>();

            ClusterClient = _clientServiceProvider.GetRequiredService<IClusterClient>();

            _consulOptions = _clientServiceProvider.GetRequiredService<IOptions<ConsulOptions>>();

            KubernetesOptions = _siloHost.Services.GetRequiredService<IOptions<KubernetesSiloOptions>>();
            
            var options = _consulOptions.Value;
            ConsulKvStoreBaseAddress = $"{options.Address}/v1/kv/{options.KvRootFolder}/";
        }

        public async Task DisposeAsync()
        {
            await _siloHost.StopAsync();
            await ClusterClient.Close();
        }
    }
}