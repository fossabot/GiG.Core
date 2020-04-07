using GiG.Core.Messaging.MassTransit.Extensions;
using GiG.Core.Messaging.MassTransit.Internal;
using GiG.Core.Messaging.MassTransit.Tests.Unit.Mocks;
using GreenPipes;
using MassTransit;
using MassTransit.Context;
using MassTransit.PipeConfigurators;
using MassTransit.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Messaging.MassTransit.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class MassTransitConsumerActivityObserverTests
    {
        private readonly Mock<TracerFactory> _tracerFactoryMock;
        private readonly Mock<ConsumeContext<MockMessage>> _consumeContextMock;

        public MassTransitConsumerActivityObserverTests()
        {
            _tracerFactoryMock = new Mock<TracerFactory>();
            _consumeContextMock = new Mock<ConsumeContext<MockMessage>>();
        }

        [Fact]
        public async Task PreConsume_TracerFactoryIsNull_DoesNotThrowException()
        {
            // Arrange
            var sut = new MassTransitConsumerActivityObserver();
            
            // Act
            await sut.PreConsume<MockMessage>(_consumeContextMock.Object);

            // Assert
            _consumeContextMock.Verify(x => x.Headers, Times.Exactly(2));
            _consumeContextMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PostConsume_TracerFactoryIsNull_DoesNotThrowException()
        {
            // Arrange
            var sut = new MassTransitConsumerActivityObserver();

            // Act
            await sut.PostConsume<MockMessage>(_consumeContextMock.Object);

            // Assert
            _consumeContextMock.VerifyNoOtherCalls();
        }
    }
}
