using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Web.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.Web.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class CorrelationIdTests
    {
        private readonly TestServer _server;
        
        public CorrelationIdTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task CorrelationIdValue_GeneratedAndAddedToResponseHeader_ReturnsCorrelationId()
        {
            // Arrange
            var client = _server.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            using var response = await client.SendAsync(request);

            var headerValues = response.Headers.GetValues(Constants.Header);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);

            Assert.True(Guid.TryParse(headerValues.FirstOrDefault(), out _));
        }

        [Fact]
        public async Task CorrelationIdValue_ResponseHeaderMatchesRequestHeader_ReturnsCorrelationId()
        {
            // Arrange
            var client = _server.CreateClient();

            var requestCorrelationId = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add(Constants.Header, requestCorrelationId);

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            using var response = await client.SendAsync(request);

            var headerValues = response.Headers.GetValues(Constants.Header);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);
            Assert.Equal(requestCorrelationId, headerValues.First());
        }
    }
}