using GiG.Core.Providers.DateTime;
using GiG.Core.TokenManager.Implementation;
using GiG.Core.TokenManager.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Net.Http;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.TokenManager.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class TokenClientTests
    {
        [Fact]
        public void TokenClient_HttpClientIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClient(null, null, null, null));
            Assert.Equal("client", exception.ParamName);
        }
        
        [Fact]
        public void TokenClient_LoggerIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClient(new HttpClient(), null, null, null));
            Assert.Equal("logger", exception.ParamName);
        }
        
        [Fact]
        public void TokenClient_TokenClientOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClient(new HttpClient(), new Logger<TokenClient>(new NullLoggerFactory()), null, null));
            Assert.Equal("tokenClientOptions", exception.ParamName);
        }
        
        [Fact]
        public void TokenClient_DateTimeProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TokenClient(new HttpClient(), new Logger<TokenClient>(new NullLoggerFactory()), new TokenClientOptions(), null));
            Assert.Equal("dateTimeProvider", exception.ParamName);
        }
        
        [Fact]
        public void LoginAsync_UsernameIsNull_ThrowsArgumentException()
        {
            var tokenClient = CreateTokenClient();
            
            var exception = Assert.ThrowsAsync<ArgumentException>(() => tokenClient.LoginAsync("", "", "", default));
            Assert.Equal("username is missing.", exception.Result.Message);
        }
        
        [Fact]
        public void LoginAsync_PasswordIsNull_ThrowsArgumentException()
        {
            var tokenClient = CreateTokenClient();
            
            var exception = Assert.ThrowsAsync<ArgumentException>(() => tokenClient.LoginAsync("a", "", "", default));
            Assert.Equal("password is missing.", exception.Result.Message);
        }
        
        [Fact]
        public void LoginAsync_ScopesIsNull_ThrowsArgumentException()
        {
            var tokenClient = CreateTokenClient();
            
            var exception = Assert.ThrowsAsync<ArgumentException>(() => tokenClient.LoginAsync("a", "b", "", default));
            Assert.Equal("scopes is missing.", exception.Result.Message);
        }
        
        [Fact]
        public void RefreshTokenAsync_RefreshTokenIsNull_ThrowsArgumentException()
        {
            var tokenClient = CreateTokenClient();
            
            var exception = Assert.ThrowsAsync<ArgumentException>(() => tokenClient.RefreshTokenAsync("", default));
            Assert.Equal("refreshToken is missing.", exception.Result.Message);
        }

        private static TokenClient CreateTokenClient() =>
            new TokenClient(new HttpClient(), new Logger<TokenClient>(new NullLoggerFactory()), new TokenClientOptions(),
                new LocalDateTimeProvider());
    }
}