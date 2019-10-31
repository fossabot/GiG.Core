using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GiG.Core.Http;
using GiG.Core.Http.Security.Hmac;
using GiG.Core.Web.Security.Hmac.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GiG.Core.Web.Security.Hmac.Tests.Integration
{
    [Trait("Category","Integration")]
    public class HmacAuthenticationHandlerTests
    {
        private readonly TestServer _server;

        public HmacAuthenticationHandlerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>()
                .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")));

        }
        [Fact]
        public async Task AuthenticateAsync_NoHmacHeader_ReturnsUnauthorized()
        {
            var client = _server.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            request.Headers.Add("X-Nonce", "123");

            using var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }
        [Fact]
        public async Task AuthenticateAsync_GetValidHmacHeader_ReturnsOK()
        {
            var testHttpClient = _server.CreateClient();
            
            var client = HttpClientFactory.CreateClient(x =>
            {
                x.AddHttpMessageHandler(_server.Services.GetRequiredService<HmacDelegatingHandler>());
                x.AddHttpMessageHandler(new MockDelegatingHandler(testHttpClient));
            });
            
            using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/mock");
            request.Headers.Add("X-Nonce", "123");

            using var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_PostValidHmacHeader_ReturnsOK()
        {
            var testHttpClient = _server.CreateClient();

            var client = HttpClientFactory.CreateClient(x =>
            {
                x.AddHttpMessageHandler(_server.Services.GetRequiredService<HmacDelegatingHandler>());
                x.AddHttpMessageHandler(new MockDelegatingHandler(testHttpClient));
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/mock");
            request.Headers.Add("X-Nonce", "123");
            request.Content = new StringContent("{\"text\":\"abccccc\"}",Encoding.UTF8,"application/json");

            using var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("abccccc",await response.Content.ReadAsStringAsync());
        }
    }
}
