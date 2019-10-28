using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansClusterClientFactoryTests : IClassFixture<ClusterClientFactoryFixture>
    {
        private readonly ClusterClientFactoryFixture _clusterFixture;

        public OrleansClusterClientFactoryTests(ClusterClientFactoryFixture clusterFixture)
        {
            _clusterFixture = clusterFixture;
        }

        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedSiloName()
        {
            //Arrange
            var grainInSiloA = _clusterFixture.OrleansClusterClientFactory.GetClusterClient("ClusterA")
                .GetGrain<IClusterClientFactoryTestGrain>(Guid.NewGuid().ToString());

            var grainInSiloB = _clusterFixture.OrleansClusterClientFactory.GetClusterClient("ClusterB")
                .GetGrain<IClusterClientFactoryTestGrain>(Guid.NewGuid().ToString());

            //Act 
            var actualValueSiloA = await grainInSiloA.GetSiloNameAsync();
            var actualValueSiloB = await grainInSiloB.GetSiloNameAsync();

            //Assert
            Assert.Equal(_clusterFixture.SiloNameA, actualValueSiloA);
            Assert.Equal(_clusterFixture.SiloNameB, actualValueSiloB);
        }
        
    }
}