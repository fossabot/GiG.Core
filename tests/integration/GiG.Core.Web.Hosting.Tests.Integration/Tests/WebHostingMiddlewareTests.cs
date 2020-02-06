using GiG.Core.Web.Hosting.Tests.Integration.Mocks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class WebHostingMiddlewareTests
    {
        private readonly TestServer _server;
        private const string PathBase = "/test";

        public WebHostingMiddlewareTests()
        {
            Environment.SetEnvironmentVariable("PATH_BASE", PathBase);

            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task RequestPathBase_WebHostingMiddlewareUsePathBaseFromConfiguration_ReturnsPathBase()
        {
            // Arrange
            var client = _server.CreateClient();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{PathBase}/api/mock");
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseValue  = await response.Content.ReadAsStringAsync();
        
            Assert.Equal(PathBase, responseValue);
        }
        
        [Fact]
        public async Task RequestRemoteIP_WebHostingMiddlewareConfigureForwardedHeaders_ReturnsXForwardedFor()
        {
            // Arrange
            var client = _server.CreateClient();
            const string forwardedFor = "10.1.12.15";
            
            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock/ip");
            request.Headers.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, forwardedFor);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var ipAddress = await response.Content.ReadAsStringAsync();
            Assert.Equal(forwardedFor, ipAddress);
        }
        
        [Fact]
        public async Task RequestRemoteIP_WebHostingForwardedHeadersNotSet_ReturnsNoContentStatus()
        {
            // Arrange
            var client = _server.CreateClient();
            
            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock/ip");
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}