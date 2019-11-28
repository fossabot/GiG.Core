using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.TokenManager.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class TokenManagerFactoryTests
    {
        [Fact]
        public void TokenManagerFactory_TokenClientFactoryIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManagerFactory(null, null, null));
            Assert.Equal("tokenClientFactory", exception.ParamName);
        }
        
        [Fact]
        public void TokenManagerFactory_LoggerFactoryIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManagerFactory(null, null, null));
            Assert.Equal("tokenClientFactory", exception.ParamName);
        }
        
        [Fact]
        public void TokenManagerFactory_DateTimeProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Implementation.TokenManagerFactory(null, null, null));
            Assert.Equal("tokenClientFactory", exception.ParamName);
        }
    }
}