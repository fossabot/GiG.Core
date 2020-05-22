using GiG.Core.Orleans.Streams.Internal;
using Moq;
using OpenTelemetry.Trace;
using Orleans.Streams;
using Orleans.TestKit;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Unit.Streams
{
    [Trait("Category", "Unit")]
    public class TracingObserverTests : TestKitBase
    {
        private readonly Mock<Tracer> _tracerMock;
        private readonly Mock<IAsyncObserver<MockMessage>> _observerMock;

        public TracingObserverTests()
        {
            _tracerMock = new Mock<Tracer>();
            _observerMock = new Mock<IAsyncObserver<MockMessage>>();
        }

        [Fact]
        public async Task OnNextAsync_TracerIsNull_DoesNotThrowException()
        {
            // Arrange
            var sut = new TracingObserver<MockMessage>(_observerMock.Object);

            // Act
            await sut.OnNextAsync(new MockMessage());

            // Assert
            _observerMock.Verify(x => x.OnNextAsync(It.IsAny<MockMessage>(), It.IsAny<StreamSequenceToken>()), Times.Once);
            VerifyNotOtherCalls();
        }

        [Fact]
        public async Task OnNextAsync_WithTracing_Success()
        {
            // Arrange
            var spanMock = new Mock<TelemetrySpan>();
            _tracerMock.Setup(x => x.StartSpanFromActivity(It.IsAny<string>(), It.IsAny<Activity>(), SpanKind.Consumer, null))
                .Returns(spanMock.Object);
            var sut = new TracingObserver<MockMessage>(_observerMock.Object, _tracerMock.Object);

            // Act
            await sut.OnNextAsync(new MockMessage());

            // Assert
            _tracerMock.Verify(x => x.StartSpanFromActivity(It.IsAny<string>(), It.IsAny<Activity>(), SpanKind.Consumer, null), Times.Once);
            _observerMock.Verify(x => x.OnNextAsync(It.IsAny<MockMessage>(), It.IsAny<StreamSequenceToken>()), Times.Once);
            spanMock.Verify(x => x.End(), Times.Once);
            VerifyNotOtherCalls();
        }

        private void VerifyNotOtherCalls()
        {
            _tracerMock.VerifyNoOtherCalls();
            _observerMock.VerifyNoOtherCalls();
        }
    }
}
