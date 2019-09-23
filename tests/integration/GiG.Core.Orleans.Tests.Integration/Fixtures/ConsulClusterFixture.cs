using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Silo.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Net.Http;
using Bogus;
using Orleans.Configuration;
using Orleans.Hosting;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ConsulClusterFixture
    {
        internal readonly IHttpClientFactory HttpClientFactory;

        internal readonly IClusterClient ClusterClient;

        internal readonly IServiceProvider ClientServiceProvider;

        internal readonly string SiloName;

        public ConsulClusterFixture()
        {
            SiloName = new Faker().Random.String2(5);

            var siloHost = Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureCluster(ctx.Configuration);
                    sb.ConfigureEndpoints();
                    sb.ConfigureConsulClustering(ctx.Configuration);
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
                        x.ConfigureConsulClustering(ctx.Configuration);
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            HttpClientFactory = ClientServiceProvider.GetService<IHttpClientFactory>();

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();
        }
    }
}