using GiG.Core.Security.Cryptography;
using GiG.Core.Web.Security.Hmac.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.Security.Hmac.Tests.Unit
{
    [Trait("Category","Unit")]
    public class HmacAuthenticationHandlerTests
    {
        private readonly Mock<IOptionsMonitor<HmacRequirement>> _hmacRequirement;
        private readonly Mock<ILoggerFactory> _loggerFactory;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<UrlEncoder> _urlEncoder;
        private readonly Mock<ISystemClock> _systemClock;
        private readonly Mock<IHmacOptionsProvider> _hmacOptionsProvider;
        private readonly Mock<IHashProviderFactory> _signatureProviderFactory;
        private readonly Mock<IHashProvider> _signatureProvider;
        private readonly DefaultHttpContext _httpContext;
        private readonly DefaultHttpRequest _request;

        public HmacAuthenticationHandlerTests()
        {
            _hmacRequirement = new Mock<IOptionsMonitor<HmacRequirement>>();
            _loggerFactory = new Mock<ILoggerFactory>();
            _logger = new Mock<ILogger>();
            _urlEncoder = new Mock<UrlEncoder>();
            _systemClock = new Mock<ISystemClock>();
            _hmacOptionsProvider = new Mock<IHmacOptionsProvider>();
            _signatureProviderFactory = new Mock<IHashProviderFactory>();
            _signatureProvider = new Mock<IHashProvider>();
            _httpContext = new DefaultHttpContext();

            _request = new DefaultHttpRequest(_httpContext);
            _request.Headers.Add("X-Nonce", "test");
            _loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_logger.Object);
            _hmacOptionsProvider.Setup(x => x.GetHmacOptions()).Returns(new HmacOptions());
            _signatureProviderFactory.Setup(x => x.GetHashProvider(It.IsAny<string>())).Returns(_signatureProvider.Object);
        }

        private async Task<HmacAuthenticationHandler> BuildHandlerAsync()
        {
            var authHandler = new HmacAuthenticationHandler(_hmacRequirement.Object, _loggerFactory.Object, _urlEncoder.Object, _systemClock.Object, _hmacOptionsProvider.Object, _signatureProviderFactory.Object);
            await authHandler.InitializeAsync(new AuthenticationScheme("hmac", "HMAC", typeof(HmacAuthenticationHandler)), _httpContext);
            return authHandler;
        }

        private void VerifyCalls()
        {
            _signatureProvider.Verify(x => x.Hash(It.IsAny<string>()), Times.Once);
            _signatureProvider.VerifyNoOtherCalls();
            _signatureProviderFactory.Verify(x => x.GetHashProvider(It.IsAny<string>()), Times.Once);
            _signatureProviderFactory.VerifyNoOtherCalls();
            _hmacOptionsProvider.Verify(x => x.GetHmacOptions(), Times.Once);
            _hmacOptionsProvider.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task HmacAuthenticationHandler_MatchHmacHeader_ReturnsSuccess()
        {            
            _request.Headers.Add("Authorization", $"hmac abc");
            _signatureProvider.Setup(x => x.Hash(It.IsAny<string>())).Returns("abc");
            var hmacAuthHandler = await BuildHandlerAsync();

            var result = await hmacAuthHandler.AuthenticateAsync();

            Assert.True(result.Succeeded);
            VerifyCalls();
        }

        [Fact]
        public async Task HmacAuthenticationHandler_MatchHmacHeader_ReturnsFail()
        {

            _request.Headers.Add("Authorization", $"hmac abc");
            _signatureProvider.Setup(x => x.Hash(It.IsAny<string>())).Returns("abcd");
            var hmacAuthHandler = await BuildHandlerAsync();

            var result = await hmacAuthHandler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.NotNull(result.Failure);
            Assert.Equal("Hmac does not match", result.Failure.Message);
            VerifyCalls();

        }

        [Fact]
        public async Task HmacAuthenticationHandler_MatchHmacHeader_ReturnsNoResult()
        {
            var hmacAuthHandler = await BuildHandlerAsync();

            var result = await hmacAuthHandler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.True(result.None);
            VerifyCalls();

        }
    }
}
