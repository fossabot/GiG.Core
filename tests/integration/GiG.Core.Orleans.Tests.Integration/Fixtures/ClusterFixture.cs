using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.Context.Orleans.Messaging;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Grains;
using GiG.Core.Orleans.Tests.Integration.Mocks;
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
                .UseOrleans(x =>
                {
                    x.ConfigureEndpoints();
                    x.UseLocalhostClustering();
                    x.AddAssemblies(typeof(EchoTestGrain));
                    x.AddAssemblies(typeof(PublisherGrain));
                    x.AddSimpleMessageStreamProvider("SMSProvider");
                    x.AddMemoryGrainStorage("PubSubStore");
                })
                .ConfigureServices(x =>
                {
                    x.AddCorrelationAccessor();
                    x.AddRequestContextAccessor();
                    x.AddScoped<IMessagePublisher<MockMessage>, MessagePublisher<MockMessage>>();
                })
                .Build();

            siloHost.StartAsync().GetAwaiter().GetResult();

            var clientHost = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ICorrelationContextAccessor, MockCorrelationContextAccessor>();
                    services.AddSingleton<IRequestContextAccessor, MockRequestContextAccessor>();
                    services.AddClusterClient((x, sp) =>
                    {
                        x.AddCorrelationOutgoingFilter(sp);
                        x.AddRequestContextOutgoingFilter(sp);
                        x.UseLocalhostClustering();
                        x.AddAssemblies(typeof(IEchoTestGrain));
                        x.AddAssemblies(typeof(IPublisherGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();
        }
    }
}