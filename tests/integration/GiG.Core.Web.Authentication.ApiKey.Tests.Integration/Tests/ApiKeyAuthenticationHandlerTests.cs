using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Integration
{
    [Trait("Category", "Integration")]
    [Trait("Feature", "ApiKeyAuthentication")]
    public class ApiKeyAuthenticationHandlerTests
    {
        private TestServer _server;

        public ApiKeyAuthenticationHandlerTests()
        {
            {
                _server = new TestServer(new WebHostBuilder()
                    .UseStartup<MockStartup>()
                    .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")));
            }
        }

        [Fact]
        public async Task AuthenticateAsync_NoApiKeyHeader_ReturnsUnauthorized()
        {
            //Arrange
            var client = _server.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            
            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("abcdef")]
        [InlineData("ab")]
        [InlineData("ABC")]
        [InlineData("")]
        public async Task AuthenticateAsync_WrongApiKeyHeader_ReturnsUnauthorized(string apiKeyHeaderValue)
        {
            //Arrange
            var client = _server.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Headers.ApiKey, apiKeyHeaderValue);
           
            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_CorrectApiKeyHeader_ReturnsOk()
        {
            //Arrange
            var client = _server.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Headers.ApiKey, "abc");
            
            //Act
            using var response = await client.SendAsync(request);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
