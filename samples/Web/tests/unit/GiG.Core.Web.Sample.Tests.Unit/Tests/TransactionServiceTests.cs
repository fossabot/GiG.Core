using GiG.Core.Context.Abstractions;
using GiG.Core.Web.Sample.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace GiG.Core.Web.Sample.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class TransactionServiceTests
    {
        private readonly Mock<AbstractLogger<TransactionService>> _logger;
        private readonly Mock<IRequestContextAccessor> _requestContextAccessor;
        private readonly TransactionService _sut;

        public TransactionServiceTests()
        {
            _logger = new Mock<AbstractLogger<TransactionService>>();
            _requestContextAccessor = new Mock<IRequestContextAccessor>();

            _sut = new TransactionService(_logger.Object, _requestContextAccessor.Object);
        }

        [Theory]
        [InlineData(-1.23)]
        [InlineData(0)]
        [InlineData(1.23)]
        public void Deposit_MultipleAmounts_ReturnsCorrectBalance(decimal depositAmount)
        {
            // Arrange
            var initialBalance = _sut.Balance;

            // Act
            var response = _sut.Deposit(depositAmount);

            // Assert
            Assert.Equal(initialBalance + depositAmount, response);
            _logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<Exception>(), It.IsAny<string>()), Times.Exactly(2));
            _requestContextAccessor.Verify(x => x.IPAddress, Times.Once);

            _logger.VerifyNoOtherCalls();
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

            // Act
            var response = _sut.Withdraw(withdrawalAmount);

            // Assert
            Assert.Equal(initialBalance - withdrawalAmount, response);
            _logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);

            _logger.VerifyNoOtherCalls();
            _requestContextAccessor.VerifyNoOtherCalls();
        }
    }
}
