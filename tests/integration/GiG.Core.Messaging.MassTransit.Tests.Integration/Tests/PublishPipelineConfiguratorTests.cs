using GiG.Core.Messaging.MassTransit.Extensions;
using GiG.Core.Messaging.MassTransit.Tests.Integration.Mocks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Messaging.MassTransit.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class PublishPipelineConfiguratorTests
    {
        private IServiceProvider _serviceProvider;
        private readonly Uri _faultAddress = new Uri("rabbitmq://localhost:15672/testdlx");

        public PublishPipelineConfiguratorTests()
        {
            SetupServices();
        }

        [Fact]
        public async Task PublishPipelineConfigurator_UseFaultAddress_FaultAddressSetInConsumeContext()
        {
            // Arrange
            var busControl = _serviceProvider.GetRequiredService<IBusControl>();
            busControl.Start();

            var messageId = Guid.NewGuid();

            // Act
            await busControl.Publish<MockMessage>(new MockMessage { Id = messageId });
            Thread.Sleep(500);
            
            // Assert
            Assert.Contains(messageId, State.FaultAddresses.Keys);
            Assert.Equal(_faultAddress.ToString(), State.FaultAddresses[messageId]);
        }
     
        private void SetupServices()
        {
            var services = new ServiceCollection();
            SetupMassTransit(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private IServiceCollection SetupMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x => {
                x.AddConsumer<MockConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                   {
                       cfg.UseFaultAddress<MockMessage>(_faultAddress);

                       cfg.ReceiveEndpoint(typeof(MockConsumer).FullName, e =>
                        {
                            e.Consumer<MockConsumer>(provider);
                        });
                   })
                );
            });

            return services;
        }
    }
}