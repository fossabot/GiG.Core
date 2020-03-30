using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Unit
{
    [Trait("Category", "Unit")]
    [Trait("Feature", "ApiKeyAuthentication")]
    public class ApiKeyAuthenticationHandlerTests
    {
        private readonly IOptionsMonitor<ApiKeyAuthenticationOptions> _apiKeyAuthenticationOptions;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UrlEncoder _urlEncoder;
        private readonly ISystemClock _systemClock;
        private readonly Mock<IAuthorizedApiKeysProvider> _authorizedKeysProviderMock;
        private readonly Dictionary<string, string> _defaultAuthorizedApiKeys;

        public ApiKeyAuthenticationHandlerTests()
        {
            // Mock the authentication handler dependencies
            _apiKeyAuthenticationOptions = new Mock<IOptionsMonitor<ApiKeyAuthenticationOptions>>().Object;

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            _loggerFactory = loggerFactoryMock.Object;

            _urlEncoder = new Mock<UrlEncoder>().Object;
            _systemClock = new Mock<ISystemClock>().Object;

            // Set up a default mock for the AuthorizedApiKeysProvider with fixed keys
            _defaultAuthorizedApiKeys = new Dictionary<string, string>
            {
                {"a9f051a6-ba2f-412b-b335-41534cc1c1cf","App1"},
                {"06f5d50c-89de-4e0d-9653-559029003b07","App2"}
            };

            _authorizedKeysProviderMock = new Mock<IAuthorizedApiKeysProvider>();
            
            _authorizedKeysProviderMock.Setup(x => x.GetAuthorizedApiKeysAsync()).Returns(Task.FromResult(_defaultAuthorizedApiKeys));
        }

        [Fact]
        public async Task ApiKeyAuthenticationHandler_NoApiKeyHeader_ReturnsNoResult()
        {
            // Arrange
            var httpContext = BuildHttpContext();
            var apiKeyAuthenticationHandler = await BuildHandlerAsync(httpContext);

            // Act
            var authenticateResult = await apiKeyAuthenticationHandler.AuthenticateAsync();

            // Assert
            _authorizedKeysProviderMock.Verify(provider => provider.GetAuthorizedApiKeysAsync(), Times.Never);
            _authorizedKeysProviderMock.VerifyNoOtherCalls();

            Assert.False(authenticateResult.Succeeded);
            Assert.True(authenticateResult.None);
        }

        [Fact]
        public async Task ApiKeyAuthenticationHandler_EmptyApiKeyHeader_ReturnsNoResult()
        {
            // Arrange
            var httpContext = BuildHttpContext(string.Empty);
            var apiKeyAuthenticationHandler = await BuildHandlerAsync(httpContext);

            // Act
            var authenticateResult = await apiKeyAuthenticationHandler.AuthenticateAsync();

            // Assert
            _authorizedKeysProviderMock.Verify(provider => provider.GetAuthorizedApiKeysAsync(), Times.Never);
            
            Assert.False(authenticateResult.Succeeded);
            Assert.False(authenticateResult.None);
            Assert.NotNull(authenticateResult.Failure);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("19f051a6-ba2f-412b-b335-41534cc1c1cf")]
        [InlineData(" a9f051a6-ba2f-412b-b335-41534cc1c1cf")]
        [InlineData("A9F051A6-BA2F-412B-B335-41534CC1C1CF")]
        [InlineData("a9f051a6ba2f412bb33541534cc1c1cf")]
        [InlineData("App1")]
        public async Task ApiKeyAuthenticationHandler_WrongApiKeyHeader_ReturnsFailure(string headerValue)
        {
            // Arrange
            var httpContext = BuildHttpContext(headerValue);
            var apiKeyAuthenticationHandler = await BuildHandlerAsync(httpContext);

            // Act
            var authenticateResult = await apiKeyAuthenticationHandler.AuthenticateAsync();

            // Assert
            _authorizedKeysProviderMock.Verify(provider => provider.GetAuthorizedApiKeysAsync(), Times.Once);
            _authorizedKeysProviderMock.VerifyNoOtherCalls();

            Assert.False(authenticateResult.Succeeded);
            Assert.False(authenticateResult.None);
            Assert.NotNull(authenticateResult.Failure);
        }

        [Fact]
        public async Task ApiKeyAuthenticationHandler_MatchApiKeyHeader_ReturnsSuccessWithTicket()
        {
            // Arrange
            var httpContext = BuildHttpContext(_defaultAuthorizedApiKeys.Keys.First());
            var apiKeyAuthenticationHandler = await BuildHandlerAsync(httpContext);

            // Act
            var authenticateResult = await apiKeyAuthenticationHandler.AuthenticateAsync();
            var claim = authenticateResult?.Ticket?.Principal?.Claims?.SingleOrDefault();

            // Assert
            Assert.True(authenticateResult.Succeeded);
            
            Assert.NotNull(claim);
            Assert.Equal("tenant_id", claim.Type);
            Assert.Equal(_defaultAuthorizedApiKeys.Values.First(), claim.Value);

            _authorizedKeysProviderMock.Verify(provider => provider.GetAuthorizedApiKeysAsync(), Times.Once);
            _authorizedKeysProviderMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(NoAuthorizedApiKeys))]
        public async Task ApiKeyAuthenticationHandler_NoAuthorizedApiKeys_ReturnsFailure(Dictionary<string,string> authorizedApiKeys)
        {
            // Arrange
            var httpContext = BuildHttpContext("abc");
            var apiKeyAuthenticationHandler = await BuildHandlerAsync(httpContext);

            _authorizedKeysProviderMock.Setup(x => x.GetAuthorizedApiKeysAsync()).Returns(Task.FromResult(authorizedApiKeys));

            // Act
            var authenticateResult = await apiKeyAuthenticationHandler.AuthenticateAsync();

            // Assert
            _authorizedKeysProviderMock.Verify(provider => provider.GetAuthorizedApiKeysAsync(), Times.Once);
            _authorizedKeysProviderMock.VerifyNoOtherCalls();

            Assert.False(authenticateResult.Succeeded);
            Assert.False(authenticateResult.None);
            Assert.NotNull(authenticateResult.Failure);
        }

        public static IEnumerable<object[]> NoAuthorizedApiKeys =>
           new List<object[]>
           {
               new object[] { null },
               new object[] { new Dictionary<string,string>(){ } },
               new object[] {new Dictionary<string,string>(){ {string.Empty, "1" } } },
               new object[] {new Dictionary<string,string>(){ {string.Empty, null} } }
           };

        private DefaultHttpContext BuildHttpContext(string apiKeyHeaderValue = null)
        {
            var httpContext = new DefaultHttpContext(); 
            var httpRequest = new DefaultHttpRequest(httpContext);
            
            if (apiKeyHeaderValue != null)
            {
                httpRequest.Headers.Add(Headers.ApiKey, apiKeyHeaderValue);
            }

            return httpContext;
        }

        private async Task<ApiKeyAuthenticationHandler> BuildHandlerAsync(DefaultHttpContext httpContext)
        {
            var authHandler = new ApiKeyAuthenticationHandler(_apiKeyAuthenticationOptions, _authorizedKeysProviderMock.Object, _loggerFactory, _urlEncoder, _systemClock);

            var authScheme = new AuthenticationScheme(ApiKeyAuthenticationOptions.DefaultScheme, ApiKeyAuthenticationOptions.DefaultScheme, typeof(ApiKeyAuthenticationHandler));
            await authHandler.InitializeAsync(authScheme, httpContext);
            
            return authHandler;
        }

    }
}
