using Bogus;
using GiG.Core.Context.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansTests : IClassFixture<DefaultClusterFixture>
    {
        private readonly DefaultClusterFixture _fixture;

        public OrleansTests(DefaultClusterFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedInteger()
        {
            //Arrange
            var grain = _fixture.ClusterClient.GetGrain<IEchoTestGrain>(Guid.NewGuid().ToString());
            var expectedValue = new Randomizer().Int();
            await grain.SetValueAsync(expectedValue);

            //Act 
            var actualValue = await grain.GetValueAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task GetIPAddressAsync_CallGrain_ReturnsExpectedIPAddress()
        {
            //Arrange
            var grain = _fixture.ClusterClient.GetGrain<IRequestContextTestGrain>(Guid.NewGuid().ToString());
            var requestContextAccessor = _fixture.ClientServiceProvider.GetRequiredService<IRequestContextAccessor>();
            var expectedValue = requestContextAccessor.IPAddress;

            //Act 
            var actualValue = await grain.GetIPAddressAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}