using GiG.Core.Providers.DateTime.Abstractions;
using GiG.Core.TokenManager.Abstractions.Interfaces;
using GiG.Core.TokenManager.Abstractions.Models;
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

        public TokenManagerFactory([NotNull] ITokenClientFactory tokenClientFactory, [NotNull] ILoggerFactory loggerFactory, [NotNull] IDateTimeProvider dateTimeProvider)  
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
        public ITokenManager Create([NotNull] TokenManagerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.Client == null) throw new ArgumentNullException(nameof(options.Client));
            if (string.IsNullOrWhiteSpace(options.Client.AuthorityUrl)) throw new ArgumentException($"{nameof(options.Client.AuthorityUrl)} is missing.");
            if (string.IsNullOrWhiteSpace(options.Client.ClientId)) throw new ArgumentException($"{nameof(options.Client.ClientId)} is missing.");

            return new TokenManager(_tokenClientFactory, _loggerFactory.CreateLogger<TokenManager>(), options, _dateTimeProvider);
        }
    }
}