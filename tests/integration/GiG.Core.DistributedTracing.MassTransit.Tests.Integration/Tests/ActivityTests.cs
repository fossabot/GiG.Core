using GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Mocks;
using GiG.Core.Messaging.MassTransit.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ActivityTests
    {
        private IServiceProvider _serviceProvider;

        public ActivityTests()
        {
            SetupServices();
        }

        [Fact]
        public async Task Activity_StartedAndAddedToContext_ReturnsActivityId()
        {
            // Arrange
            var busControl = _serviceProvider.GetRequiredService<IBusControl>();
            busControl.Start();

            var messageId = Guid.NewGuid();
            var activity = new System.Diagnostics.Activity("producer");
            activity.Start();
            var expectedActivityRoot = activity.RootId;

            // Act
            await busControl.Publish(new MockMessage { Id = messageId });
            await Task.Delay(500);

            // Assert
            Assert.Contains(messageId, State.Messages.Keys);
            Assert.Equal(expectedActivityRoot, State.Messages[messageId]);
        }

        [Fact]
        public async Task Activity_NotStartedAddedToContext_ReturnsActivityId()
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
            AddMessageConsumer(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private static void AddMessageConsumer(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ActivityMockConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.ConfigurePublish(x => x.UseActivityFilter());
                    cfg.Host.AddActivityConsumerObserver();

                    cfg.ReceiveEndpoint(typeof(ActivityMockConsumer).FullName, e =>
                    {
                        e.Consumer<ActivityMockConsumer>(provider);
                    });
                }));
            });
        }
    }
}