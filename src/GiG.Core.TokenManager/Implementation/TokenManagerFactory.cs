using GiG.Core.Providers.DateTime.Abstractions;
using GiG.Core.TokenManager.Interfaces;
using GiG.Core.TokenManager.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.TokenManager.Tests.Unit")]
namespace GiG.Core.TokenManager.Implementation
{
    /// <inheritdoc />
    internal class TokenManagerFactory : ITokenManagerFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenClientFactory _tokenClientFactory;
        private readonly ILoggerFactory _loggerFactory;

        public TokenManagerFactory(ITokenClientFactory tokenClientFactory, ILoggerFactory loggerFactory, [NotNull] IDateTimeProvider dateTimeProvider)  
        {
            _tokenClientFactory = tokenClientFactory ?? throw new ArgumentNullException(nameof(tokenClientFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        /// <inheritdoc />
        public ITokenManager Create(Action<ITokenManagerBuilder> configurationActions = null)
        {
            var configurator = new TokenManagerBuilder();

            configurationActions?.Invoke(configurator);

            return Create(configurator.Config);
        }

        /// <inheritdoc />
        public ITokenManager Create(TokenManagerOptions config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (config.Client == null) throw new ArgumentNullException(nameof(config.Client));
            if (string.IsNullOrWhiteSpace(config.Client.AuthorityUrl)) throw new ArgumentException($"{nameof(config.Client.AuthorityUrl)} is missing.");
            if (string.IsNullOrWhiteSpace(config.Client.ClientId)) throw new ArgumentException($"{nameof(config.Client.ClientId)} is missing.");

            return new TokenManager(_tokenClientFactory, _loggerFactory.CreateLogger<TokenManager>(), config, _dateTimeProvider);
        }
    }
}