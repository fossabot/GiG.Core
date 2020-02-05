using GiG.Core.DistributedTracing.MassTransit.Extensions;
using GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Mocks;
using GiG.Core.Messaging.MassTransit.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            // Act
            await busControl.Publish(new MockMessage { Id = messageId }, x => x.CorrelationId = correlationId);
            await Task.Delay(500);

            // Assert
            Assert.Contains(messageId, State.Messages.Keys);
            Assert.Equal(correlationId.ToString(), State.Messages[messageId]);
        }

        [Fact]
        public async Task CorrelationIdValue_NotAddedToContext_ReturnsCorrelationId()
        {
            // Arrange
            var busControl = _serviceProvider.GetRequiredService<IBusControl>();
            busControl.Start();

            var messageId = Guid.NewGuid();

            // Act
            await busControl.Publish(new MockMessage { Id = messageId });
            await Task.Delay(500);

            // Assert
            Assert.Contains(messageId, State.Messages.Keys);
            Assert.NotEqual(Guid.Empty.ToString(), State.Messages[messageId]);
        }

        private void SetupServices()
        {
            State.Init();
            var services = new ServiceCollection();
            services.AddCorrelationAccessor();
            AddMessageConsumer(services);
           
            _serviceProvider = services.BuildServiceProvider();
        }

        private static void AddMessageConsumer(IServiceCollection services)
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
        }
    }
}