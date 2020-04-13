using GiG.Core.Messaging.MassTransit.Internal;
using GiG.Core.Messaging.MassTransit.Tests.Unit.Mocks;
using MassTransit;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Messaging.MassTransit.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class MassTransitConsumerActivityObserverTests
    {
        private readonly Mock<ConsumeContext<MockMessage>> _consumeContextMock;

        public MassTransitConsumerActivityObserverTests()
        {
            _consumeContextMock = new Mock<ConsumeContext<MockMessage>>();
        }

        [Fact]
        public async Task PreConsume_TracerFactoryIsNull_DoesNotThrowException()
        {
            // Arrange
            var sut = new MassTransitConsumerActivityObserver();
            
            // Act
            await sut.PreConsume(_consumeContextMock.Object);

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
            await sut.PostConsume(_consumeContextMock.Object);

            // Assert
            _consumeContextMock.VerifyNoOtherCalls();
        }
    }
}
