using Bogus;
using GiG.Core.Orleans.Client.Abstractions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ClusterClientFactoryFixture :  IAsyncLifetime
    {
        internal IClusterClientFactory OrleansClusterClientFactory;

        internal IServiceProvider ClientServiceProvider;

        internal string SiloNameA;
        
        internal string SiloNameB;

        private IHost _siloHostA;
 
        private IHost _siloHostB;

        public async Task InitializeAsync()
        {
            SiloNameA = new Faker().Random.String2(5);

            _siloHostA = Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterFactoryA"));
                    sb.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:ClusterFactoryA:Silo"));
                    sb.ConfigureConsulClustering(ctx.Configuration.GetSection("Orleans:ConsulMembershipProvider"));
                    sb.AddAssemblies(typeof(ClusterClientFactoryTestGrain));
                    sb.Configure<SiloOptions>(options => options.SiloName = SiloNameA);
                })
                .Build();
            await _siloHostA.StartAsync();

            SiloNameB = new Faker().Random.String2(5);
            _siloHostB = Host.CreateDefaultBuilder()
               .UseOrleans((ctx, sb) =>
               {
                   sb.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterFactoryB"));
                   sb.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:ClusterFactoryB:Silo"));
                   sb.ConfigureConsulClustering(ctx.Configuration.GetSection("Orleans:ConsulMembershipProvider"));
                   sb.AddAssemblies(typeof(ClusterClientFactoryTestGrain));
                   sb.Configure<SiloOptions>(options => options.SiloName = SiloNameB);
               })
               .Build();
            await _siloHostB.StartAsync();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHttpClient();

                    services.AddClusterClientFactory()
                        .AddClusterClient("ClusterA", () =>
                        {
                            return services.CreateClusterClient(builder =>
                            {
                                builder.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterFactoryA"));
                                builder.ConfigureConsulClustering(ctx.Configuration.GetSection("Orleans:ConsulMembershipProvider"));
                                builder.AddAssemblies(typeof(IClusterClientFactoryTestGrain));
                            });
                        })
                        .AddClusterClient("ClusterB", () => {
                            return services.CreateClusterClient(builder =>
                            {
                                builder.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterFactoryB"));
                                builder.ConfigureConsulClustering(ctx.Configuration.GetSection("Orleans:ConsulMembershipProvider"));
                                builder.AddAssemblies(typeof(IClusterClientFactoryTestGrain));
                            });
                        });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;
            OrleansClusterClientFactory = ClientServiceProvider.GetRequiredService<IClusterClientFactory>();
        }
        
        public async Task DisposeAsync()
        {
            if (_siloHostA != null)
            {
                await _siloHostA.StopAsync();
            }

            if (_siloHostB != null)
            {
                await _siloHostB.StopAsync();
            }

            OrleansClusterClientFactory?.Dispose();
        }
    }
}