using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Orleans;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Streams.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ClusterFixture
    {
        internal readonly IClusterClient ClusterClient;

        internal readonly IServiceProvider ClientServiceProvider;

        public ClusterFixture()
        {
            var siloHost = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .UseOrleans((ctx, x) =>
                {
                    x.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:ClusterA:Silo"));
                    x.UseLocalhostClustering();
                    x.AddAssemblies(typeof(EchoTestGrain));
                    x.AddSimpleMessageStreamProvider("SMSProvider");
                    x.AddMemoryGrainStorage("PubSubStore");
                })
                .ConfigureServices(x =>
                {
                    x.AddCorrelationAccessor();
                    x.AddRequestContextAccessor();
                    x.AddStream();
                })
                .Build();

            siloHost.StartAsync().GetAwaiter().GetResult();

            var clientHost = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
                    services.AddSingleton<IRequestContextAccessor, MockRequestContextAccessor>();
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.AddCorrelationOutgoingFilter(sp);
                        x.AddRequestContextOutgoingFilter(sp);
                        x.UseLocalhostClustering();
                        x.AddAssemblies(typeof(IEchoTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();
        }
    }
}