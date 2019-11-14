using Bogus;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using GiG.Core.Orleans.Sample.Grains;
using GiG.Core.Orleans.Streams.Abstractions;
using Moq;
using Orleans.Streams;
using Orleans.TestKit;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Sample.Tests.Unit.Tests
{
    public class WalletGrainTests : TestKitBase
    {
        private readonly Mock<IStreamFactory> _streamFactoryMock;
        private readonly Mock<IStream<WalletTransaction>> _streamMock;

        public WalletGrainTests()
        {
            _streamFactoryMock = Silo.AddServiceProbe<IStreamFactory>();
            _streamMock = new Mock<IStream<WalletTransaction>>();
        }

        [Fact]
        public async Task Wallet_GetBalance()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var amount = new Randomizer().Decimal(0, 1000);

            // Act
            var state = Silo.State<BalanceState>();
            state.Amount = amount;
            var grain = await Silo.CreateGrainAsync<WalletGrain>(grainId);
            var actualBalance = await grain.GetBalanceAsync();
            var storageStats = this.Silo.StorageStats();

            // Assert
            Assert.Equal(amount, actualBalance);
            Assert.Equal(0, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            _streamFactoryMock.Verify(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace), Times.Once);
            VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Wallet_Credit()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var amount = new Randomizer().Decimal(0, 1000);
     
            _streamFactoryMock.Setup(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace))
                .Returns(_streamMock.Object);

            // Act
            var grain = await Silo.CreateGrainAsync<WalletGrain>(grainId);
            var actualBalance = await grain.CreditAsync(amount);
            var storageStats = this.Silo.StorageStats();

            // Assert
            Assert.Equal(1, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            Assert.Equal(amount, actualBalance);
            _streamFactoryMock.Verify(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace), Times.Once);
            _streamMock.Verify(x => x.PublishAsync(It.IsAny<WalletTransaction>(), It.IsAny<StreamSequenceToken>()), Times.Once());
            VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Wallet_Debit()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var amount = new Randomizer().Decimal(0, 1000);

            _streamFactoryMock.Setup(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace))
                .Returns(_streamMock.Object);

            // Act
            var grain = await Silo.CreateGrainAsync<WalletGrain>(grainId);
            var actualBalance = await grain.DebitAsync(amount);
            var storageStats = this.Silo.StorageStats();

            // Assert
            Assert.Equal(1, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            Assert.Equal(amount * -1, actualBalance);
            _streamFactoryMock.Verify(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace), Times.Once);
            _streamMock.Verify(x => x.PublishAsync(It.IsAny<WalletTransaction>(), It.IsAny<StreamSequenceToken>()), Times.Once());
            VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Wallet_Stream_DepositMessage()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var transactionMessage = new Faker<PaymentTransaction>()
                   .RuleFor(x => x.Amount, new Randomizer().Decimal(0, 1000))
                   .RuleFor(x => x.TransactionType, PaymentTransactionType.Deposit)
                   .Generate();

            _streamFactoryMock.Setup(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace))
                .Returns(_streamMock.Object);

            // Act
            var grain = await Silo.CreateGrainAsync<WalletGrain>(grainId);
            await grain.OnNextAsync(transactionMessage);
            var storageStats = this.Silo.StorageStats();
            var balanceState = this.Silo.State<BalanceState>();

            // Assert
            Assert.Equal(1, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            Assert.Equal(transactionMessage.Amount, balanceState.Amount);
            _streamFactoryMock.Verify(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace), Times.Once);
            _streamMock.Verify(x => x.PublishAsync(It.Is<WalletTransaction>(x => x.TransactionType == WalletTransactionType.Credit), It.IsAny<StreamSequenceToken>()), Times.Once());
            VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Wallet_Stream_WithdrawMessage()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var transactionMessage = new Faker<PaymentTransaction>()
                   .RuleFor(x => x.Amount, new Randomizer().Decimal(0, 1000))
                   .RuleFor(x => x.TransactionType, PaymentTransactionType.Withdrawal)
                   .Generate();

            _streamFactoryMock.Setup(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace))
                .Returns(_streamMock.Object);

            // Act
            var grain = await Silo.CreateGrainAsync<WalletGrain>(grainId);
            await grain.OnNextAsync(transactionMessage);
            var storageStats = this.Silo.StorageStats();
            var balanceState = this.Silo.State<BalanceState>();

            // Assert
            Assert.Equal(1, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            Assert.Equal(transactionMessage.Amount * -1, balanceState.Amount);
            _streamFactoryMock.Verify(x => x.GetStream<WalletTransaction>(It.IsAny<IStreamProvider>(), grainId, Constants.WalletTransactionsStreamNamespace), Times.Once);
            _streamMock.Verify(x => x.PublishAsync(It.Is<WalletTransaction>(x => x.TransactionType == WalletTransactionType.Debit), It.IsAny<StreamSequenceToken>()), Times.Once());
            VerifyNoOtherCalls();
        }

        private void VerifyNoOtherCalls()
        {
            _streamFactoryMock.VerifyNoOtherCalls();
            _streamMock.VerifyNoOtherCalls();
        }
    }
}
