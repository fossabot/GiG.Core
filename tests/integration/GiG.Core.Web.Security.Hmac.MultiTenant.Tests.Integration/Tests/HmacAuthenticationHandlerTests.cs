using GiG.Core.Http.Security.Hmac.Extensions;
using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.Security.Hmac.Abstractions;
using GiG.Core.Web.Security.Hmac.MultiTenant.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GiG.Core.Http;
using GiG.Core.Http.Security.Hmac;
using GiG.Core.Security.Cryptography;
using Xunit;
using Constants = GiG.Core.Security.Hmac.Abstractions.Constants;

namespace GiG.Core.Web.Security.Hmac.MultiTenant.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HmacAuthenticationHandlerTests
    {
        private readonly TestServer _server;
        const string DefaultClientName = "Default";
        const string DefaultClient2Name = "Default2";
        public HmacAuthenticationHandlerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHttpClient(DefaultClientName)
                    .ConfigurePrimaryHttpMessageHandler(x => _server.CreateHandler())
                    .ConfigureHttpClient(x => x.BaseAddress = _server.BaseAddress)
                    .AddHmacDelegatingHandler()
                    .ConfigureDefaultHmacDelegatingHandlerOptionProvider(ctx.Configuration.GetSection("HmacClientDefault"))
                    .ConfigureHttpClient(x=>x.DefaultRequestHeaders.Add(Core.MultiTenant.Abstractions.Constants.Header,"1"));

                    services.AddHttpClient(DefaultClient2Name)
                    .ConfigurePrimaryHttpMessageHandler(x => _server.CreateHandler())
                    .ConfigureHttpClient(x => x.BaseAddress = _server.BaseAddress)
                    .AddHmacDelegatingHandler()
                    .ConfigureDefaultHmacDelegatingHandlerOptionProvider(ctx.Configuration.GetSection("HmacClientDefault2"))
                    .ConfigureHttpClient(x=>x.DefaultRequestHeaders.Add(Core.MultiTenant.Abstractions.Constants.Header,"2"));

                    services.AddHttpClient("NonHmacClient")
                        .ConfigurePrimaryHttpMessageHandler(x => _server.CreateHandler())
                        .ConfigureHttpClient(x => x.BaseAddress = _server.BaseAddress);
                })
                .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")));
            ;

        }

        [Fact]
        public async Task AuthenticateAsync_WithRespectiveHmac_ReturnsNoContent()
        {
            //Arrange
            var client = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClientName);
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.NonceHeader, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }



        [Fact]
        public async Task AuthenticateAsync_WithInvalidHmac_ReturnsUnauthorized()
        {
            //Arrange
            var client = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient("NonHmacClient");
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.NonceHeader, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_TwoClientsDifferentSecret_ReturnsNoContent()
        {
            //Arrange
            var clientDefault = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClientName);
            var clientDefault2 = _server.Services.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClient2Name);
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.NonceHeader, "123");

            using var request2 = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request2.Headers.Add(Constants.NonceHeader, "123");

            //Act
            using var response = await clientDefault.SendAsync(request);
            using var response2 = await clientDefault2.SendAsync(request2);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response2.StatusCode);

        }
    }
}
