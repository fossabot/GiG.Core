using GiG.Core.Authentication.Hmac.Abstractions;
using GiG.Core.Security.Cryptography;
using GiG.Core.Web.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Authentication.Hmac.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HmacAuthenticationHandlerTests
    {
        private readonly Mock<IOptionsMonitor<HmacRequirement>> _hmacRequirement;
        private readonly Mock<ILoggerFactory> _loggerFactory;
        private readonly Mock<UrlEncoder> _urlEncoder;
        private readonly Mock<ISystemClock> _systemClock;
        private readonly Mock<IHmacOptionsProvider> _optionsProvider;
        private readonly Mock<IHashProviderFactory> _hashProviderFactory;
        private readonly Mock<IHashProvider> _hashProvider;
        private readonly Mock<IHmacSignatureProvider> _signatureProvider;
        private readonly DefaultHttpContext _httpContext;
        private readonly DefaultHttpRequest _request;

        public HmacAuthenticationHandlerTests()
        {
            _hmacRequirement = new Mock<IOptionsMonitor<HmacRequirement>>();
            _loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            _urlEncoder = new Mock<UrlEncoder>();
            _systemClock = new Mock<ISystemClock>();
            _optionsProvider = new Mock<IHmacOptionsProvider>();
            _hashProviderFactory = new Mock<IHashProviderFactory>();
            _hashProvider = new Mock<IHashProvider>();
            _signatureProvider = new Mock<IHmacSignatureProvider>();
            _httpContext = new DefaultHttpContext();

            _signatureProvider.Setup(x => x.GetSignature(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("abc");
            _request = new DefaultHttpRequest(_httpContext);
            _request.Headers.Add(Headers.Nonce, "test");
            _loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            _optionsProvider.Setup(x => x.GetHmacOptions()).Returns(new HmacOptions());
            _hashProviderFactory.Setup(x => x.GetHashProvider(It.IsAny<string>())).Returns(_hashProvider.Object);
        }

        [Fact]
        public async Task HmacAuthenticationHandler_MatchHmacHeader_ReturnsSuccess()
        {
            //Arrange
            _request.Headers.Add(Headers.Authorization, "hmac abc");
            _hashProvider.Setup(x => x.Hash(It.IsAny<string>())).Returns("abc");
            var hmacAuthHandler = await BuildHandlerAsync();

            //Act
            var result = await hmacAuthHandler.AuthenticateAsync();

            //Assert
            Assert.True(result.Succeeded);
            VerifyCallsWithHeader();
        }
        
        [Fact]
        public async Task HmacAuthenticationHandler_NonceHeaderMissing_ReturnsFail()
        {
            //Arrange
            _request.Headers.Remove(Headers.Nonce);
            _request.Headers.Add(Headers.Authorization, "hmac abc");
            _hashProvider.Setup(x => x.Hash(It.IsAny<string>())).Returns("abc");
            var hmacAuthHandler = await BuildHandlerAsync();

            //Act
            var result = await hmacAuthHandler.AuthenticateAsync();

            //Assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Failure);
            Assert.Equal("Nonce not set.", result.Failure.Message);
            VerifyCallsNoHeader();
        }

        [Fact]
        public async Task HmacAuthenticationHandler_MatchHmacHeader_ReturnsFail()
        {
            //Arrange
            _request.Headers.Add(Headers.Authorization, "hmac abc");
            _hashProvider.Setup(x => x.Hash(It.IsAny<string>())).Returns("abcd");
            var hmacAuthHandler = await BuildHandlerAsync();

            //Act
            var result = await hmacAuthHandler.AuthenticateAsync();

            //Assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Failure);
            Assert.Equal("Hmac does not match.", result.Failure.Message);
            VerifyCallsWithHeader();
        }

        [Fact]
        public async Task HmacAuthenticationHandler_MatchHmacHeader_ReturnsFailed()
        {
            //Arrange
            var hmacAuthHandler = await BuildHandlerAsync();

            //Act
            var result = await hmacAuthHandler.AuthenticateAsync();

            //Assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Failure);
            VerifyCallsNoHeader();
        }

        private async Task<HmacAuthenticationHandler> BuildHandlerAsync()
        {
            var authHandler = new HmacAuthenticationHandler(_hmacRequirement.Object, _loggerFactory.Object, _urlEncoder.Object, _systemClock.Object, _optionsProvider.Object, _hashProviderFactory.Object, _signatureProvider.Object);
            await authHandler.InitializeAsync(new AuthenticationScheme("hmac", "HMAC", typeof(HmacAuthenticationHandler)), _httpContext);
            return authHandler;
        }

        private void VerifyCallsNoHeader()
        {
            _optionsProvider.Verify(x => x.GetHmacOptions(), Times.Once);
            _optionsProvider.VerifyNoOtherCalls();
        }

        private void VerifyCallsWithHeader()
        {
            _hashProvider.Verify(x => x.Hash(It.IsAny<string>()), Times.Once);
            _hashProvider.VerifyNoOtherCalls();
            _hashProviderFactory.Verify(x => x.GetHashProvider(It.IsAny<string>()), Times.Once);
            _hashProviderFactory.VerifyNoOtherCalls();
            _optionsProvider.Verify(x => x.GetHmacOptions(), Times.Once);
            _optionsProvider.VerifyNoOtherCalls();
        }
    }
}
