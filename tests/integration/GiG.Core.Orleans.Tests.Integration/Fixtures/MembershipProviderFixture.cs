using Bogus;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Configurations;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
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
    public class MembershipProviderFixture
    {
        internal readonly IOptions<ConsulOptions> ConsulOptions;

        internal readonly IClusterClient ClusterClient;

        internal readonly IHttpClientFactory HttpClientFactory;

        internal readonly IServiceProvider ClientServiceProvider;

        internal readonly string SiloName;

        public MembershipProviderFixture()
        {
            SiloName = new Faker().Random.String2(5);

            var siloHost = Host.CreateDefaultBuilder()
                 .UseOrleans((ctx, sb) =>
                 {
                     sb.ConfigureCluster(ctx.Configuration);
                     sb.ConfigureEndpoints(11112, 30001);
                     sb.UseMembershipProvider(ctx.Configuration, x =>
                      {
                          x.ConfigureConsulClustering(ctx.Configuration);
                          x.ConfigureKubernetesClustering(ctx.Configuration);
                      });
                     sb.AddAssemblies(typeof(EchoTestGrain));
                     sb.Configure<SiloOptions>(options => options.SiloName = SiloName);
                 })
                 .Build();

            siloHost.StartAsync().GetAwaiter().GetResult();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {                 
                    services.AddHttpClient();
                    services.AddClusterClient(x =>
                    {
                        x.ConfigureCluster(ctx.Configuration);
                        x.UseMembershipProvider(ctx.Configuration, builder =>
                        {
                            builder.ConfigureConsulClustering(ctx.Configuration);
                            builder.ConfigureKubernetesClustering(ctx.Configuration);
                        });
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            HttpClientFactory = ClientServiceProvider.GetRequiredService<IHttpClientFactory>();

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();

            ConsulOptions = ClientServiceProvider.GetRequiredService<IOptions<ConsulOptions>>();
        }
    }
}