using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ClusterFixture
    {
        internal readonly IClusterClient ClusterClient;

        public ClusterFixture()
        {
            var siloHost = new HostBuilder()
                .UseOrleans(x =>
                {
                    x.ConfigureEndpoints();
                    x.UseLocalhostClustering();
                    x.AddAssemblies(typeof(TestGrain));
                })
                .Build();

            siloHost.StartAsync().GetAwaiter().GetResult();

            var clientHost = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddClusterClient((x, sp) =>
                    {
                        x.UseLocalhostClustering();
                        x.AddAssemblies(typeof(ITestGrain));
                    });
                })
                .Build();

            ClusterClient = clientHost.Services.GetRequiredService<IClusterClient>();
        }
    }
}