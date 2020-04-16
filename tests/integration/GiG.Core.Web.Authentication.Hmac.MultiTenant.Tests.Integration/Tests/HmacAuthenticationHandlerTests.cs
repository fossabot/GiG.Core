using GiG.Core.Http.Authentication.Hmac.Extensions;
using GiG.Core.Web.Authentication.Hmac.MultiTenant.Tests.Integration.Mocks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Constants = GiG.Core.Authentication.Hmac.Abstractions.Constants;

namespace GiG.Core.Web.Authentication.Hmac.MultiTenant.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HmacAuthenticationHandlerTests : IClassFixture<TestFixture>
    {
        private const string DefaultClientName = "Default";
        private const string DefaultClient2Name = "Default2";
        private readonly ServiceProvider _services;

        public HmacAuthenticationHandlerTests(TestFixture fixture)
        {
            var testServer = fixture.Host.GetTestServer();

            var configuration = testServer.Services.GetRequiredService<IConfiguration>();
            var serviceCollection = fixture.ServiceCollection;

            serviceCollection.AddHttpClient(DefaultClientName)
                .ConfigurePrimaryHttpMessageHandler(x => testServer.CreateHandler())
                .ConfigureHttpClient(x => x.BaseAddress = testServer.BaseAddress)
                .AddHmacDelegatingHandler()
                .ConfigureDefaultHmacDelegatingHandlerOptionProvider(configuration.GetSection("HmacClientDefault"))
                .ConfigureHttpClient(x=>x.DefaultRequestHeaders.Add(Core.MultiTenant.Abstractions.Constants.Header,"1"));

            serviceCollection.AddHttpClient(DefaultClient2Name)
                .ConfigurePrimaryHttpMessageHandler(x => testServer.CreateHandler())
                .ConfigureHttpClient(x => x.BaseAddress = testServer.BaseAddress)
                .AddHmacDelegatingHandler()
                .ConfigureDefaultHmacDelegatingHandlerOptionProvider(configuration.GetSection("HmacClientDefault2"))
                .ConfigureHttpClient(x=>x.DefaultRequestHeaders.Add(Core.MultiTenant.Abstractions.Constants.Header,"2"));

            serviceCollection.AddHttpClient("NonHmacClient")
                .ConfigurePrimaryHttpMessageHandler(x => testServer.CreateHandler())
                .ConfigureHttpClient(x => x.BaseAddress = testServer.BaseAddress);

            _services = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task AuthenticateAsync_WithRespectiveHmac_ReturnsNoContent()
        {
            //Arrange
           var client = _services.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClientName);
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidHmac_ReturnsUnauthorized()
        {
            //Arrange
            var client = _services.GetRequiredService<IHttpClientFactory>().CreateClient("NonHmacClient");
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_TwoClientsDifferentSecret_ReturnsNoContent()
        {
            //Arrange
            var clientDefault = _services.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClientName);
            var clientDefault2 = _services.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClient2Name);
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Constants.Nonce, "123");

            using var request2 = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request2.Headers.Add(Constants.Nonce, "123");

            //Act
            using var response = await clientDefault.SendAsync(request);
            using var response2 = await clientDefault2.SendAsync(request2);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response2.StatusCode);
        }
    }
}
