using Bogus;
using GiG.Core.Orleans.Client;
using GiG.Core.Orleans.Client.Abstractions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ClusterClientFactoryFixture
    {
        internal readonly IClusterClientFactory OrleansClusterClientFactory;

        internal readonly IServiceProvider ClientServiceProvider;

        internal readonly string SiloNameA;
        
        internal readonly string SiloNameB;

        public ClusterClientFactoryFixture()
        {
            SiloNameA = new Faker().Random.String2(5);

            var siloHostA = Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterA"));
                    sb.ConfigureEndpoints();
                    sb.ConfigureConsulClustering(ctx.Configuration);
                    sb.AddAssemblies(typeof(ClusterClientFactoryTestGrain));
                    sb.Configure<SiloOptions>(options => options.SiloName = SiloNameA);
                })
                .Build();
            siloHostA.StartAsync().GetAwaiter().GetResult();

            SiloNameB = new Faker().Random.String2(5);
            var siloHostB = Host.CreateDefaultBuilder()
               .UseOrleans((ctx, sb) =>
               {
                   sb.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterB"));
                   sb.ConfigureEndpoints(11112, 30002);
                   sb.ConfigureConsulClustering(ctx.Configuration);
                   sb.AddAssemblies(typeof(ClusterClientFactoryTestGrain));
                   sb.Configure<SiloOptions>(options => options.SiloName = SiloNameB);
               })
               .Build();
            siloHostB.StartAsync().GetAwaiter().GetResult();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHttpClient();

                    services.AddClusterClientFactory()
                        .AddClusterClient("ClusterA", () =>
                        {
                            return services.CreateClusterClient((builder) =>
                            {
                                builder.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterA"));
                                builder.ConfigureConsulClustering(ctx.Configuration);
                                builder.AddAssemblies(typeof(IClusterClientFactoryTestGrain));
                            });
                        })
                        .AddClusterClient("ClusterB", () => {
                            return services.CreateClusterClient((builder) =>
                            {
                                builder.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterB"));
                                builder.ConfigureConsulClustering(ctx.Configuration);
                                builder.AddAssemblies(typeof(IClusterClientFactoryTestGrain));
                            });
                        });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;
            OrleansClusterClientFactory = ClientServiceProvider.GetRequiredService<IClusterClientFactory>();
        }
    }
}