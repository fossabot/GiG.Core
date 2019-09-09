using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace GiG.Core.Web.Hosting.Tests.Integration
{
    public class WebHostingMiddlewareTests
    {
        private readonly TestServer _server;

        public WebHostingMiddlewareTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()
                )
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task WebHostingMiddlewareConfigurePathBase()
        {
            // Arrange
            var client = _server.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock");
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseDictionary  = await response.Content.ReadAsStringAsync();
            var responseValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseDictionary);
            Assert.Equal("/api/mock", responseValues["value"]);
        }
        
        [Fact]
        public async Task WebHostingMiddlewareConfigureXForwardedHeaders()
        {
            // Arrange
            var client = _server.CreateClient();
            var forwardedFor = "10.1.12.15";
            
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock/ip");
            request.Headers.Add(ForwardedHeaders.XForwardedFor.ToString(), forwardedFor );
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(forwardedFor, response.RequestMessage.Headers.GetValues(ForwardedHeaders.XForwardedFor.ToString()).FirstOrDefault());
        }
    }
}