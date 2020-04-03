using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansClusterClientFactoryTests : IClassFixture<ClusterClientFactoryFixture>
    {
        private readonly ClusterClientFactoryFixture _fixture;

        public OrleansClusterClientFactoryTests(ClusterClientFactoryFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedSiloName()
        {
            //Arrange
            var grainInSiloA = _fixture.OrleansClusterClientFactory.Get("ClusterA")
                .GetGrain<IClusterClientFactoryTestGrain>(Guid.NewGuid().ToString());

            var grainInSiloB = _fixture.OrleansClusterClientFactory.Get("ClusterB")
                .GetGrain<IClusterClientFactoryTestGrain>(Guid.NewGuid().ToString());

            //Act 
            var actualValueSiloA = await grainInSiloA.GetSiloNameAsync();
            var actualValueSiloB = await grainInSiloB.GetSiloNameAsync();

            //Assert
            Assert.Equal(_fixture.SiloNameA, actualValueSiloA);
            Assert.Equal(_fixture.SiloNameB, actualValueSiloB);
        }

    }
}