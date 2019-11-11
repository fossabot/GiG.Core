using GiG.Core.DistributedTracing.MassTransit.Extensions;
using GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Mocks;
using GiG.Core.DistributedTracing.Web.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class CorrelationIdTests
    {
        private IServiceProvider _serviceProvider;

        public CorrelationIdTests()
        {
            SetupServices();
        }

        [Fact]
        public async Task CorrelationIdValue_GeneratedAndAddedToContext_ReturnsCorrelationId()
        {
            // Arrange
            var busControl = _serviceProvider.GetRequiredService<IBusControl>();
            busControl.Start();

            var messageId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();

            await busControl.Publish<MockMessage>(new MockMessage { Id = messageId }, x => x.CorrelationId = correlationId);
            Thread.Sleep(500);
            
            // Assert
            Assert.Contains(messageId, State.Messages.Keys);
            Assert.Equal(correlationId, State.Messages[messageId]);
        }

        [Fact]
        public async Task CorrelationIdValue_NotAddedToContext_ReturnsCorrelationId()
        {
            // Arrange
            var busControl = _serviceProvider.GetRequiredService<IBusControl>();
            busControl.Start();

            var messageId = Guid.NewGuid();

            await busControl.Publish<MockMessage>(new MockMessage { Id = messageId });
            Thread.Sleep(500);
            // Assert
            Assert.Contains(messageId, State.Messages.Keys);
            Assert.NotEqual(Guid.Empty, State.Messages[messageId]);
        }

        private void SetupServices()
        {
            var services = new ServiceCollection();
            services.AddMassTransitContext();
            services.AddCorrelationAccessor();
            AddMessageConsumer(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private IServiceCollection AddMessageConsumer(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<MockConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.Host.AddDefaultConsumerObserver(x.Collection.BuildServiceProvider());

                    cfg.ReceiveEndpoint(typeof(MockConsumer).FullName, e =>
                    {
                        e.Consumer<MockConsumer>(provider);
                    });

                }));
            });

            return services;
        }
    }
}