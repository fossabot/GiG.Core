using Bogus;
using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansTests : DefaultClusterLifetime
    {
        public OrleansTests()
        {
        }

        //[Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedInteger()
        {
            //Arrange
            var grain = ClusterClient.GetGrain<IEchoTestGrain>(Guid.NewGuid().ToString());
            var expectedValue = new Randomizer().Int();
            await grain.SetValueAsync(expectedValue);

            //Act 
            var actualValue = await grain.GetValueAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
        
        //[Fact]
        public async Task GetCorrelationIdAsync_CallGrain_ReturnsExpectedCorrelationGuid()
        {
            //Arrange
            var grain = ClusterClient.GetGrain<ICorrelationTestGrain>(Guid.NewGuid().ToString());
            var correlationAccessor = ClientServiceProvider.GetRequiredService<ICorrelationContextAccessor>();
            var expectedValue = correlationAccessor.Value;

            //Act 
            var actualValue = await grain.GetCorrelationIdAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        //[Fact]
        public async Task GetIPAddressAsync_CallGrain_ReturnsExpectedIPAddress()
        {
            //Arrange
            var grain = ClusterClient.GetGrain<IRequestContextTestGrain>(Guid.NewGuid().ToString());
            var requestContextAccessor = ClientServiceProvider.GetRequiredService<IRequestContextAccessor>();
            var expectedValue = requestContextAccessor.IPAddress;

            //Act 
            var actualValue = await grain.GetIPAddressAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}