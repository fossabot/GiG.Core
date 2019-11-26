using GiG.Core.TokenManager.Implementation;
using GiG.Core.TokenManager.Models;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.TokenManager.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class TokenClientFactoryTests : IClassFixture<Fixtures.TokenManagerFixture>
    {
        private readonly Fixtures.TokenManagerFixture _tokenManagerFixture;
        public TokenClientFactoryTests(Fixtures.TokenManagerFixture tokenManagerFixture)
        {
            _tokenManagerFixture = tokenManagerFixture;
        }
        
        [Fact]
        public void TokenClientFactory_HttpClientFactoryIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClientFactory(null, null, null));
            Assert.Equal("httpClientFactory", exception.ParamName);
        }
        
        [Fact]
        public void TokenClientFactory_LoggerFactoryIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClientFactory(_tokenManagerFixture.HttpClientFactory, null, null));
            Assert.Equal("loggerFactory", exception.ParamName);
        }
        
        [Fact]
        public void TokenClientFactory_DateTimeProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClientFactory(_tokenManagerFixture.HttpClientFactory, new LoggerFactory(), null));
            Assert.Equal("dateTimeProvider", exception.ParamName);
        }
        
        [Fact]
        public void CreateClient_TokenClientOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _tokenManagerFixture.GetTokenClientFactory().CreateClient(null));
            Assert.Equal("tokenClientOptions", exception.ParamName);
        }
        
        [Fact]
        public void CreateClient_TokenClientOptionsPropertiesMayBeNull_ThrowsArgumentException()
        {
            var tokenClientOptions = new TokenClientOptions();
            
            var exception = Assert.Throws<ArgumentException>(() => _tokenManagerFixture.GetTokenClientFactory().CreateClient(tokenClientOptions));
            Assert.Equal("AuthorityUrl is missing.", exception.Message);

            tokenClientOptions.AuthorityUrl = "localhost";
            exception = Assert.Throws<ArgumentException>(() => _tokenManagerFixture.GetTokenClientFactory().CreateClient(tokenClientOptions));
            Assert.Equal("ClientId is missing.", exception.Message);
        }
    }
}