using GiG.Core.Context.Web.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Context.Web.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class RequestContextTests
    {
        private readonly TestServer _server;
        
        public RequestContextTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task RequestContext_IPAddress_SameAsXForwardedFor()
        {
            // Arrange
            var client = _server.CreateClient();
            var forwardedFor = "10.1.12.15";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock");
            request.Headers.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, forwardedFor);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var ipAddress = await response.Content.ReadAsStringAsync();
            Assert.Equal(forwardedFor, ipAddress);
        }
    }
}