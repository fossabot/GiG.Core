using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Web.Sample.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GiG.Core.Web.Sample.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class TransactionServiceTests
    {
        private readonly Mock<IRequestContextAccessor> _requestContextAccessor;
        private readonly Mock<IActivityContextAccessor> _activityContextAccessor;
        private readonly TransactionService _sut;

        public TransactionServiceTests()
        {
            var logger = new Mock<ILogger<TransactionService>>();
            _requestContextAccessor = new Mock<IRequestContextAccessor>();
            _activityContextAccessor = new Mock<IActivityContextAccessor>();

            _sut = new TransactionService(logger.Object, _requestContextAccessor.Object, _activityContextAccessor.Object);
        }

        [Theory]
        [InlineData(-1.23)]
        [InlineData(0)]
        [InlineData(1.23)]
        public void Deposit_MultipleAmounts_ReturnsCorrectBalance(decimal depositAmount)
        {
            // Arrange
            var initialBalance = _sut.Balance;
            var expectedResponse = initialBalance + depositAmount;

            // Act
            var response = _sut.Deposit(depositAmount);

            // Assert
            Assert.Equal(expectedResponse, response);
            _requestContextAccessor.Verify(x => x.IPAddress, Times.Once);
            _activityContextAccessor.Verify(x => x.OperationName, Times.Once);
            _activityContextAccessor.Verify(x => x.CorrelationId, Times.Once);
            _activityContextAccessor.Verify(x => x.SpanId, Times.Once);
            _activityContextAccessor.Verify(x => x.TraceId, Times.Once);

            _activityContextAccessor.VerifyNoOtherCalls();
            _requestContextAccessor.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(-1.23)]
        [InlineData(0)]
        [InlineData(1.23)]
        public void Withdraw_MultipleAmounts_ReturnsCorrectBalance(decimal withdrawalAmount)
        {
            // Arrange
            var initialBalance = _sut.Balance;
            var expectedResponse = initialBalance - withdrawalAmount;

            // Act
            var response = _sut.Withdraw(withdrawalAmount);

            // Assert
            Assert.Equal(expectedResponse, response);

            _activityContextAccessor.VerifyNoOtherCalls();
            _requestContextAccessor.VerifyNoOtherCalls();
        }
    }
}
