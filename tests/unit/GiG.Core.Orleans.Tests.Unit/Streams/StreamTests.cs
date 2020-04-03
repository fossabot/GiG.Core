using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams;
using Moq;
using OpenTelemetry.Trace.Configuration;
using Orleans.Streams;
using Orleans.TestKit;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Unit.Streams
{
    [Trait("Category", "Unit")]
    public class StreamTests : TestKitBase
    {
        private readonly Mock<TracerFactory> _tracerFactoryMock;
        private readonly Mock<IAsyncStream<MockMessage>> _streamMock;
        private readonly Mock<IActivityContextAccessor> _activityContextAccessorMock;

        public StreamTests()
        {
            _tracerFactoryMock = new Mock<TracerFactory>();
            _streamMock = new Mock<IAsyncStream<MockMessage>>();
            _activityContextAccessorMock = new Mock<IActivityContextAccessor>();
        }

        [Fact]
        public async Task PublishAsync_TracerFactoryIsNull_DoesNotThrowException()
        {
            // Arrange
            var sut = new Stream<MockMessage>(_streamMock.Object, _activityContextAccessorMock.Object);

            // Act
            await sut.PublishAsync(new MockMessage());

            // Assert
            _streamMock.Verify(x => x.OnNextAsync(It.IsAny<MockMessage>(), It.IsAny<StreamSequenceToken>()), Times.Once);
            VerifyNotOtherCalls();
        }

        private void VerifyNotOtherCalls()
        {
            _tracerFactoryMock.VerifyNoOtherCalls();
            _activityContextAccessorMock.VerifyNoOtherCalls();
            _streamMock.VerifyNoOtherCalls();
        }
    }
}
