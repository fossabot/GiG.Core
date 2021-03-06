﻿using GiG.Core.Providers.DateTime.Abstractions;
using GiG.Core.TokenManager.Abstractions.Interfaces;
using GiG.Core.TokenManager.Abstractions.Models;
using GiG.Core.TokenManager.Exceptions;
using IdentityModel.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TokenClientOptions = GiG.Core.TokenManager.Abstractions.Models.TokenClientOptions;

[assembly: InternalsVisibleTo("GiG.Core.TokenManager.Tests.Unit")]
namespace GiG.Core.TokenManager.Implementation
{
    internal class TokenClient : ITokenClient
    {
        private readonly HttpClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;
        private readonly IDiscoveryCache _discoveryCache;
        private readonly TokenClientOptions _tokenClientOptions;

        internal TokenClient([NotNull] HttpClient client, [NotNull] ILogger<TokenClient> logger, [NotNull] TokenClientOptions tokenClientOptions, [NotNull] IDateTimeProvider dateTimeProvider)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenClientOptions = tokenClientOptions ?? throw new ArgumentNullException(nameof(tokenClientOptions));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            
            _discoveryCache = new DiscoveryCache(_tokenClientOptions.AuthorityUrl, () => client, new DiscoveryPolicy
            {
                RequireHttps = _tokenClientOptions.RequireHttps
            });
        }

        /// <inheritdoc />
        public async Task<DiscoveryResult> GetDiscoveryAsync()
        {
            LogEntry(LogLevel.Debug, Constants.Logs.DiscoveryRetrieving);

            var discoveryResponse = await _discoveryCache.GetAsync();

            if (discoveryResponse?.IsError == true)
            {
                LogEntry(LogLevel.Error, discoveryResponse.Error, discoveryResponse.Exception);

                throw new TokenManagerException(discoveryResponse.Error);
            }

            if (string.IsNullOrEmpty(discoveryResponse?.TokenEndpoint))
            {
                LogEntry(LogLevel.Error, Constants.Errors.TokenEndpointNullError);

                throw new TokenManagerException(Constants.Errors.TokenEndpointNullError);
            }

            LogEntry(LogLevel.Debug, Constants.Logs.DiscoveryRetrieved);

            return new DiscoveryResult
            {
                TokenEndpoint = discoveryResponse.TokenEndpoint
            };
        }

        /// <inheritdoc />
        public async Task<TokenResult> LoginAsync([NotNull] string username, [NotNull] string password, [NotNull] string scopes,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"'{nameof(username)}' must not be null, empty or whitespace.", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException($"'{nameof(password)}' must not be null, empty or whitespace.", nameof(password));
            if (string.IsNullOrWhiteSpace(scopes)) throw new ArgumentException($"'{nameof(scopes)}' must not be null, empty or whitespace.", nameof(scopes));

            LogEntry(LogLevel.Debug, Constants.Logs.AccessTokenRetrieving);

            var discoveryResponse = await GetDiscoveryAsync();

            var tokenResponse = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                ClientId = _tokenClientOptions.ClientId,
                ClientSecret = _tokenClientOptions.ClientSecret,
                Scope = scopes,
                Address = discoveryResponse.TokenEndpoint,
                UserName = username,
                Password = password
            }, cancellationToken);

            return ToTokenResult(tokenResponse, username);
        }

        /// <inheritdoc />
        public Task<TokenResult> LoginAsync(string username, string password,
            CancellationToken cancellationToken = default) =>
            LoginAsync(username, password, _tokenClientOptions.Scopes, cancellationToken);

        /// <inheritdoc />
        public async Task<TokenResult> RefreshTokenAsync([NotNull] string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentException($"'{nameof(refreshToken)}' must not be null, empty or whitespace.", nameof(refreshToken));

            LogEntry(LogLevel.Debug, Constants.Logs.RefreshTokenRetrieving);

            var discoveryResponse = await GetDiscoveryAsync();

            var tokenResponse =
                await _client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Scope = _tokenClientOptions.Scopes,
                    ClientId = _tokenClientOptions.ClientId,
                    ClientSecret = _tokenClientOptions.ClientSecret,
                    Address = discoveryResponse.TokenEndpoint,
                    RefreshToken = refreshToken
                }, cancellationToken);

            return ToTokenResult(tokenResponse);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private TokenResult ToTokenResult(TokenResponse tokenResponse, string username = null)
        {
            if (tokenResponse?.IsError == true)
            {
                if (tokenResponse.Error == Constants.Errors.InvalidGrant)
                {
                    LogEntry(LogLevel.Information, $"Unauthorized - {tokenResponse.ErrorDescription}");

                    throw new TokenManagerException(tokenResponse.ErrorDescription);
                }

                LogEntry(LogLevel.Error, tokenResponse.ErrorDescription, username: username);

                throw new TokenManagerException(tokenResponse.Error);
            }

            if (string.IsNullOrEmpty(tokenResponse?.AccessToken))
            {
                LogEntry(LogLevel.Error, Constants.Errors.AccessTokenNullError, username: username);

                throw new TokenManagerException(Constants.Errors.AccessTokenNullError);
            }

            LogEntry(LogLevel.Debug, Constants.Logs.AccessTokenRetrieved, username: username);

            return new TokenResult
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                ExpiresAt = _dateTimeProvider.Now.AddSeconds(tokenResponse.ExpiresIn)
            };
        }

        private void LogEntry(LogLevel logLevel, string message, Exception ex = null, string username = null)
        {
            using (_logger.BeginScope(new Dictionary<string, object>{
                    ["AuthorityUrl"] = _tokenClientOptions.AuthorityUrl,
                    ["ClientId"] = _tokenClientOptions.ClientId,
                    ["Scopes"] = _tokenClientOptions.Scopes,
                    ["Username"] = username
                }))
                {
                    _logger.Log(logLevel, ex, message);
                }
        }
    }
}