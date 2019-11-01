using GiG.Core.Http;
using GiG.Core.Http.Security.Hmac;
using GiG.Core.Http.Security.Hmac.Extensions;
using GiG.Core.Security.Http;
using GiG.Core.Web.Security.Hmac.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Security.Hmac.Tests.Integration.Tests
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
            services.AddHttpClient("Default")
            .ConfigurePrimaryHttpMessageHandler(x => _server.CreateHandler())
            .ConfigureHttpClient(x => x.BaseAddress = _server.BaseAddress)
            .AddClientHmacAuthentication()
            .ConfigureDefaultHmacDelegatingHandlerOptionProvider(ctx.Configuration.GetSection("Hmac")))
                .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")));
            ;

        }

        [Fact]
        public async Task AuthenticateAsync_NoHmacHeader_ReturnsUnauthorized()
        {
            //Arrange
            var client = _server.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(HmacConstants.NonceHeader, "123");

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
            request.Headers.Add(HmacConstants.NonceHeader, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_PostValidHmacHeader_ReturnsOK()
        {
            //Arrange
            var client = HttpClientFactory.Create(x =>
            {
                x.AddDelegatingHandler(_server.Services.GetRequiredService<HmacDelegatingHandler>());
                x.WithMessageHandler(_server.CreateHandler());
                x.Options.WithBaseAddress(_server.BaseAddress);
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/mock");
            request.Headers.Add(HmacConstants.NonceHeader, "123");
            request.Content = new StringContent("{\"text\":\"abccccc\"}", Encoding.UTF8, "application/json");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("abccccc", await response.Content.ReadAsStringAsync());
        }
    }
}
