using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
            var responseValue = await response.Content.ReadAsStringAsync();
            Assert.Equal("/api/mock", response.Content.ReadAsStreamAsync());
        }
    }
}