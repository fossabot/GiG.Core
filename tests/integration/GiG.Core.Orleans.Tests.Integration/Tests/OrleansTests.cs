using Bogus;
using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _clusterFixture;

        public OrleansTests(ClusterFixture clusterFixture)
        {
            _clusterFixture = clusterFixture;
        }

        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedInteger()
        {
            //Arrange
            var grain = _clusterFixture.ClusterClient.GetGrain<IEchoTestGrain>(Guid.NewGuid().ToString());
            var expectedValue = new Randomizer().Int();
            await grain.SetValueAsync(expectedValue);

            //Act 
            var actualValue = await grain.GetValueAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
        
        [Fact]
        public async Task GetCorrelationIdAsync_CallGrain_ReturnsExpectedCorrelationGuid()
        {
            //Arrange
            var grain = _clusterFixture.ClusterClient.GetGrain<ICorrelationTestGrain>(Guid.NewGuid().ToString());
            var correlationAccessor = _clusterFixture.ClientServiceProvider.GetRequiredService<ICorrelationContextAccessor>();
            var expectedValue = correlationAccessor.Value;

            //Act 
            var actualValue = await grain.GetCorrelationIdAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task GetIPAddressAsync_CallGrain_ReturnsExpectedIPAddress()
        {
            //Arrange
            var grain = _clusterFixture.ClusterClient.GetGrain<IRequestContextTestGrain>(Guid.NewGuid().ToString());
            var requestContextAccessor = _clusterFixture.ClientServiceProvider.GetRequiredService<IRequestContextAccessor>();
            var expectedValue = requestContextAccessor.IPAddress;

            //Act 
            var actualValue = await grain.GetIPAddressAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}