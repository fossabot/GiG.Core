using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.FluentValidation.Tests.Integration
{
    public class FluentValidationMiddlewareTests
    {
        private readonly TestServer _server;
        
        public FluentValidationMiddlewareTests()
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

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}