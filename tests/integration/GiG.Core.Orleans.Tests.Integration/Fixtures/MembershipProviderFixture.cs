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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net.Http;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public abstract class MembershipProviderFixture
    {
        internal readonly IOptions<ConsulOptions> ConsulOptions;

        internal readonly IOptions<KubernetesSiloOptions> KubernetesOptions;

        internal readonly IClusterClient ClusterClient;

        internal readonly IHttpClientFactory HttpClientFactory;

        internal readonly IServiceProvider ClientServiceProvider;

        internal readonly string SiloName;

        public MembershipProviderFixture(string membershipProviderSectionName)
        {
            SiloName = new Faker().Random.String2(5);

            var siloHost = Host.CreateDefaultBuilder()
                 .ConfigureServices((ctx, services) => 
                 {
                     var membershipProviderSection = ctx.Configuration.GetSection(membershipProviderSectionName);
                     services.Configure<KubernetesSiloOptions>(membershipProviderSection);
                 })
                 .UseOrleans((ctx, sb) =>
                 {
                     var membershipProviderSection = ctx.Configuration.GetSection(membershipProviderSectionName);
                     
                     sb.ConfigureCluster(ctx.Configuration);
                     sb.ConfigureEndpoints(ctx.Configuration);
                     sb.UseMembershipProvider(membershipProviderSection, x =>
                      {
                          x.ConfigureConsulClustering(membershipProviderSection);
                          x.ConfigureKubernetesClustering(membershipProviderSection);
                      });
                     sb.AddAssemblies(typeof(EchoTestGrain));
                     sb.Configure<SiloOptions>(options => options.SiloName = SiloName);
                 })
                 .Build();

            siloHost.StartAsync().GetAwaiter().GetResult();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    var membershipProviderSection = ctx.Configuration.GetSection(membershipProviderSectionName);

                    services.AddHttpClient();
                    services.AddDefaultClusterClient(x =>
                    {
                        x.ConfigureCluster(ctx.Configuration);
                        x.UseMembershipProvider(membershipProviderSection, builder =>
                        {
                            builder.ConfigureConsulClustering(membershipProviderSection);
                            builder.ConfigureKubernetesClustering(membershipProviderSection);
                        });
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            HttpClientFactory = ClientServiceProvider.GetRequiredService<IHttpClientFactory>();

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();

            ConsulOptions = ClientServiceProvider.GetRequiredService<IOptions<ConsulOptions>>();

            KubernetesOptions = siloHost.Services.GetRequiredService<IOptions<KubernetesSiloOptions>>();
        }
    }
}