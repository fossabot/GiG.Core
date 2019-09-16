using Bogus;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    public class OrleansTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _clusterFixture;

        public OrleansTests(ClusterFixture clusterFixture)
        {
            _clusterFixture = clusterFixture;
        }

        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsInt()
        {
            //Arrange
            var grain = _clusterFixture.ClusterClient.GetGrain<ITestGrain>(Guid.NewGuid().ToString());
            var expectedValue = new Randomizer().Int();
            await grain.SetValueAsync(expectedValue);

            //Act 
            var actualValue = await grain.GetValueAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}