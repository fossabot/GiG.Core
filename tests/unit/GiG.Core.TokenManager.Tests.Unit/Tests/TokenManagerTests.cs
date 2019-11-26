using GiG.Core.TokenManager.Models;
using GiG.Core.TokenManager.Tests.Unit.Fixtures;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.TokenManager.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public  class TokenManagerTests : IClassFixture<TokenManagerFixture>
    {
        private readonly TokenManagerFixture _tokenManagerFixture;
        public TokenManagerTests(TokenManagerFixture tokenManagerFixture)
        {
            _tokenManagerFixture = tokenManagerFixture;
        }
        
        [Fact]
        public void TokenManager_TokenClientFactoryIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManager(null, null, null, null));
            Assert.Equal("tokenClientFactory", exception.ParamName);
        }
        
        [Fact]
        public void TokenManager_LoggerIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManager(_tokenManagerFixture.GetTokenClientFactory(), null, null, null));
            Assert.Equal("logger", exception.ParamName);
        }
        
        [Fact]
        public void TokenManager_TokenManagerOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManager(_tokenManagerFixture.GetTokenClientFactory(), _tokenManagerFixture.GetLogger(), null, null));
            Assert.Equal("tokenManagerOptions", exception.ParamName);
        }
        
        [Fact]
        public void TokenManager_DateTimeProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManager(_tokenManagerFixture.GetTokenClientFactory(), _tokenManagerFixture.GetLogger(), new TokenManagerOptions(), null));
            Assert.Equal("dateTimeProvider", exception.ParamName);
        }
    }
}