using Bogus;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Sample.Grains;
using Orleans.TestKit;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Sample.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class PaymentTransactionGrainTests : TestKitBase
    {
        [Fact]
        public async Task PaymentTransactions_GetAll_ReturnsNoTransactions()
        {
            // Arrange
            var grainId = new Randomizer().Guid();

            // Act
            var grain = await Silo.CreateGrainAsync<PaymentTransactionGrain>(grainId);
            var actualTransactions = await grain.GetAllAsync();
            var storageStats = this.Silo.StorageStats();

            // Assert
            Assert.NotNull(actualTransactions);
            Assert.Empty(actualTransactions);
            Assert.Equal(0, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
        }

        [Fact]
        public async Task PaymentTransactions_GetAll_ReturnsOneTransaction()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var transaction = new Faker<PaymentTransaction>()
                    .RuleFor(x => x.Amount, new Randomizer().Decimal(0,1000))
                    .RuleFor(x => x.TransactionType, (PaymentTransactionType)new Randomizer().Short(1,2))
                    .Generate();

            // Act
            var grain = await Silo.CreateGrainAsync<PaymentTransactionGrain>(grainId);
            await grain.OnNextAsync(transaction);
            var actualTransactions = await grain.GetAllAsync();
            var storageStats = this.Silo.StorageStats();

            // Assert
            Assert.NotNull(actualTransactions);
            Assert.Single(actualTransactions);
            Assert.Equal(0, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            Assert.Equal(transaction, actualTransactions.First());
        }

        [Fact]
        public async Task PaymentTransactions_GetAll_ReturnsTransactionList()
        {
            // Arrange
            var grainId = new Randomizer().Guid();
            var transactions = new Faker<PaymentTransaction>()
                    .RuleFor(x => x.Amount, new Randomizer().Decimal(0, 1000))
                    .RuleFor(x => x.TransactionType, (PaymentTransactionType)new Randomizer().Short(1, 2))
                    .Generate(3);

            // Act
            var grain = await Silo.CreateGrainAsync<PaymentTransactionGrain>(grainId);
            transactions.ForEach(async x => await grain.OnNextAsync(x));
            var actualTransactions = await grain.GetAllAsync();
            var storageStats = this.Silo.StorageStats();

            // Assert
            Assert.NotNull(transactions);
            Assert.Equal(transactions.Count(), actualTransactions.Count());
            Assert.Equal(0, storageStats.Writes);
            Assert.Equal(0, storageStats.Reads);
            Assert.Equal(transactions, actualTransactions);
        }
    }    
}
