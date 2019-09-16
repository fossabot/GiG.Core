using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Web.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.Web.Tests.Integration.Tests
{
    [Trait("Category", "integration")]
    public class CorrelationIdTests
    {
        private readonly TestServer _server;
        
        public CorrelationIdTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task CorrelationIdGeneratedAndAddedToResponseHeader()
        {
            // Arrange
            var client = _server.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock");
            using var response = await client.SendAsync(request);

            var headerValues = response.Headers.GetValues(Constants.Header);

            // Assert
            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);

            Assert.True(Guid.TryParse(headerValues.FirstOrDefault(), out _));
        }

        [Fact]
        public async Task CorrelationIdInResponseHeaderMatchesRequestHeader()
        {
            // Arrange
            var client = _server.CreateClient();

            var requestCorrelationId = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add(Constants.Header, requestCorrelationId);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock");
            using var response = await client.SendAsync(request);

            var headerValues = response.Headers.GetValues(Constants.Header);

            // Assert
            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);
            Assert.Equal(requestCorrelationId, headerValues.First());
        }
    }
}