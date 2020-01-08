using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansClusterClientFactoryTests : ClusterClientFactoryLifetime
    {
        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedSiloName()
        {
            //Arrange
            var grainInSiloA = OrleansClusterClientFactory.Get("ClusterA")
                .GetGrain<IClusterClientFactoryTestGrain>(Guid.NewGuid().ToString());

            var grainInSiloB = OrleansClusterClientFactory.Get("ClusterB")
                .GetGrain<IClusterClientFactoryTestGrain>(Guid.NewGuid().ToString());

            //Act 
            var actualValueSiloA = await grainInSiloA.GetSiloNameAsync();
            var actualValueSiloB = await grainInSiloB.GetSiloNameAsync();

            //Assert
            Assert.Equal(SiloNameA, actualValueSiloA);
            Assert.Equal(SiloNameB, actualValueSiloB);
        }

    }
}