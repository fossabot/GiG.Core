using Bogus;
using GiG.Core.Context.Abstractions;
using GiG.Core.Web.Sample.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace GiG.Core.Web.Sample.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class WebSampleServiceTests
    {
        private readonly Mock<AbstractLogger<TransactionService>> _logger;
        private readonly Mock<IRequestContextAccessor> _requestContextAccessor;
        private readonly TransactionService _sut;

        public WebSampleServiceTests()
        {
            _logger = new Mock<AbstractLogger<TransactionService>>();
            _requestContextAccessor = new Mock<IRequestContextAccessor>();

            _sut = new TransactionService(_logger.Object, _requestContextAccessor.Object);
        }

        [Fact]
        public void Deposit_Success()
        {
            // Arrange
            var amount = new Randomizer().Decimal();
            var initialBalance = _sut.Balance;

            // Act
            var response = _sut.Deposit(amount);

            // Assert
            Assert.Equal(initialBalance + amount, response);
            _logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<Exception>(), It.IsAny<string>()), Times.Exactly(2));
            _requestContextAccessor.Verify(x => x.IPAddress, Times.Once);
        }

        [Fact]
        public void Withdraw_Success()
        {
            // Arrange
            var amount = new Randomizer().Decimal();
            var initialBalance = _sut.Balance;

            // Act
            var response = _sut.Withdraw(amount);

            // Assert
            Assert.Equal(initialBalance - amount, response);
            _logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }
    }
}
