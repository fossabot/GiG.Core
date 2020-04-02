using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.Orleans.Streams.Tests.Integration.Fixtures;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class HealthCheckTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _fixture;

        public HealthCheckTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CombinedHealthCheck_WithKafkaHealthCheck_ReturnsHealthyStatus()
        {
            // Arrange
            var client = _fixture.Host.GetTestClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, new HealthChecksOptions().CombinedUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}