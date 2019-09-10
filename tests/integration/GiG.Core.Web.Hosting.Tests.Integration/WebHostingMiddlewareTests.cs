using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Hosting.Tests.Integration
{
    public class WebHostingMiddlewareTests
    {
        private readonly TestServer _server;
        private readonly string _pathBase = "/test";

        public WebHostingMiddlewareTests()
        {
            Environment.SetEnvironmentVariable("PATH_BASE", _pathBase);

            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task WebHostingMiddlewareConfigurePathBase()
        {
            // Arrange
            var client = _server.CreateClient();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{_pathBase}/api/mock");
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseValue  = await response.Content.ReadAsStringAsync();
        
            Assert.Equal(_pathBase, responseValue);
        }
        
        [Fact]
        public async Task WebHostingMiddlewareConfigureForwardedHeaders()
        {
            // Arrange
            var client = _server.CreateClient();
            var forwardedFor = "10.1.12.15";
            
            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock/ip");
            request.Headers.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, forwardedFor);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var ipAddress = await response.Content.ReadAsStringAsync();
            Assert.Equal(forwardedFor, ipAddress);
        }
        
        [Fact]
        public async Task WebHostingForwardedHeadersNotSet()
        {
            // Arrange
            var client = _server.CreateClient();
            
            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock/ip");
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}