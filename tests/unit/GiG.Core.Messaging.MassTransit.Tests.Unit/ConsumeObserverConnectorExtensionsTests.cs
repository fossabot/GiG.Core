using GiG.Core.Messaging.MassTransit.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace GiG.Core.Messaging.MassTransit.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ConsumeObserverConnectorExtensionsTests
    {
        [Fact]
        public void AddDefaultConsumerObserver_ConsumeObserverConnectorIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ConsumeObserverConnectorExtensions.AddDefaultConsumerObserver(null, null));
            Assert.Equal("consumeObserverConnector", exception.ParamName);
        }

        [Fact]
        public void AddDefaultConsumerObserver_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var services = new ServiceCollection();
                services.AddMassTransit(x =>
                {
                    x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                    {
                        cfg.Host.AddDefaultConsumerObserver(null);
                    }));
                });
                services.BuildServiceProvider().GetRequiredService<IBusControl>();
            });

            Assert.Equal("serviceProvider", exception.ParamName);
        }

        [Fact]
        public void AddActivityConsumerObserver_ConsumeObserverConnectorIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ConsumeObserverConnectorExtensions.AddActivityConsumerObserver(null));
            Assert.Equal("consumeObserverConnector", exception.ParamName);
        }

        [Fact]
        public void AddActivityConsumerObserverWithTracing_ConsumeObserverConnectorIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ConsumeObserverConnectorExtensions.AddActivityConsumerObserverWithTracing(null, null));
            Assert.Equal("consumeObserverConnector", exception.ParamName);
        }

        [Fact]
        public void AddActivityConsumerObserverWithTracing_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var services = new ServiceCollection();
                services.AddMassTransit(x =>
                {
                    x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                    {
                        cfg.Host.AddActivityConsumerObserverWithTracing(null);
                    }));
                });
                services.BuildServiceProvider().GetRequiredService<IBusControl>();
            });

            Assert.Equal("serviceProvider", exception.ParamName);
        }
    }
}
