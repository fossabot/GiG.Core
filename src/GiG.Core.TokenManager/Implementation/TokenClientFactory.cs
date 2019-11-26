using GiG.Core.Providers.DateTime.Abstractions;
using GiG.Core.TokenManager.Interfaces;
using GiG.Core.TokenManager.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace GiG.Core.TokenManager.Implementation
{
    /// <inheritdoc />
    internal class TokenClientFactory : ITokenClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILoggerFactory _loggerFactory;

        public TokenClientFactory([NotNull] IHttpClientFactory httpClientFactory, [NotNull] ILoggerFactory loggerFactory, [NotNull] IDateTimeProvider dateTimeProvider) 
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }
        
        /// <inheritdoc />
        public ITokenClient CreateClient([NotNull] TokenClientOptions tokenClientOptions)
        {
            if (tokenClientOptions == null) throw new ArgumentNullException(nameof(tokenClientOptions));
            if (string.IsNullOrWhiteSpace(tokenClientOptions.AuthorityUrl)) throw new ArgumentException($"{nameof(tokenClientOptions.AuthorityUrl)} is missing.");
            if (string.IsNullOrWhiteSpace(tokenClientOptions.ClientId)) throw new ArgumentException($"{nameof(tokenClientOptions.ClientId)} is missing.");
            
            return new TokenClient(_httpClientFactory.CreateClient(),
                _loggerFactory.CreateLogger<TokenClient>(),
                tokenClientOptions, _dateTimeProvider);
        }
    }
}