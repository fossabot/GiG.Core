using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Fixtures;
using Microsoft.AspNetCore.TestHost;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    [Trait("Feature", "ApiKeyAuthentication")]
    public class ApiKeyAuthenticationHandlerTests : IClassFixture<WebFixture>
    {
        private readonly WebFixture _fixture;

        public ApiKeyAuthenticationHandlerTests(WebFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AuthenticateAsync_NoApiKeyHeader_ReturnsUnauthorized()
        {
            //Arrange
            var client = _fixture.Host.GetTestClient();
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
            var client = _fixture.Host.GetTestClient();
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
            var client = _fixture.Host.GetTestClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/mock");
            request.Headers.Add(Headers.ApiKey, "abc");

            var activity = new Activity("test");
            activity.Start();

            //Act
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("1", await response.Content.ReadAsStringAsync());
        }
    }
}