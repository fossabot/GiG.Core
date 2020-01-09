using GiG.Core.Messaging.MassTransit.Extensions;
using MassTransit;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Messaging.Tests.Unit.MassTransit
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
                Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.Host.AddDefaultConsumerObserver(null);
                }));

            Assert.Equal("serviceProvider", exception.ParamName);
        }
    }
}
