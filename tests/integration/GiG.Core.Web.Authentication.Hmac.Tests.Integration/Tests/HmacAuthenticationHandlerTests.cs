using GiG.Core.Authentication.Hmac.Abstractions;
using GiG.Core.Http.Authentication.Hmac.Extensions;
using GiG.Core.Web.Authentication.Hmac.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable StringLiteralTypo

namespace GiG.Core.Web.Authentication.Hmac.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HmacAuthenticationHandlerTests
    {
        private readonly TestServer _server;

        public HmacAuthenticationHandlerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHttpClient("Default")
                    .ConfigurePrimaryHttpMessageHandler(x => _server.CreateHandler())
                    .ConfigureHttpClient(x => x.BaseAddress = _server.BaseAddress)
                    .AddHmacDelegatingHandler()
                    .ConfigureDefaultHmacDelegatingHandlerOptionProvider(ctx.Configuration.GetSection("Hmac"));

                    services.AddHttpClient("Default2")
                    .ConfigurePrimaryHttpMessageHandler(x => _server.CreateHandler())
                    .ConfigureHttpClient(x => x.BaseAddress = _server.BaseAddress)
                    .AddHmacDelegatingHandler()
                    .ConfigureDefaultHmacDelegatingHandlerOptionProvider(ctx.Configuration);
                })
                .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")));
        }

        [Fact]
        public async Task AuthenticateAsync_NoHmacHeader_ReturnsUnauthorized()
        {
            //Arrange
            var client = _server.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_GetValidHmacHeader_ReturnsOK()
        {
            //Arrange
            var client = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient("Default");

            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_PostValidHmacHeader_ReturnsOK()
        {
            //Arrange
            var client = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient("Default"); 

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");
            request.Content = new StringContent("{\"text\":\"abccccc\"}", Encoding.UTF8, "application/json");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("abccccc", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task AuthenticateAsync_MultipleInstances_ReturnsOK()
        {
            //Arrange
            var client = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient("Default");
            var clientNew = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient("Default2");

            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");
                
            using var requestNew = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            requestNew.Headers.Add(Constants.Nonce, "123");

            //Act
            using var response = await client.SendAsync(request);
            using var responseNew = await clientNew.SendAsync(requestNew);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, responseNew .StatusCode);
        }
    }
}
