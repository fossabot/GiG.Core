using GiG.Core.TokenManager.Exceptions;
using GiG.Core.TokenManager.Interfaces;
using GiG.Core.TokenManager.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.TokenManager.Implementation
{
    internal class TokenManager : ITokenManager
    {
        //private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenClientFactory _tokenClientFactory;
        private readonly ILogger _logger;
        private readonly TokenManagerOptions _tokenManagerOptions;

        private readonly SemaphoreSlim _semaphore;

        private ITokenClient _tokenClient;
        private TokenResult _lastTokenResult;
        private Timer _timer;

        private bool LastTokenValid
        {
            get
            {
                if (_lastTokenResult == null)
                {
                    return false;
                }

                return false; //_dateTimeProvider.GetNow() <= _lastTokenResult.ExpiresAt;
            }
        }
        internal TokenManager([NotNull] ITokenClientFactory tokenClientFactory, [NotNull] ILogger logger, [NotNull] TokenManagerOptions tokenManagerOptions) //IDateTimeProvider dateTimeProvider, 
        {
            _tokenClientFactory = tokenClientFactory ?? throw new ArgumentNullException(nameof(tokenClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenManagerOptions = tokenManagerOptions ?? throw new ArgumentNullException(nameof(tokenManagerOptions));
            //_dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));

            _semaphore = new SemaphoreSlim(1);
        }

        /// <inheritdoc />
        public async Task<TokenResult> GetAndRefreshTokenAsync()
        {
            if (LastTokenValid)
            {
                return _lastTokenResult;
            }

            _lastTokenResult = await RefreshTokenAsync();

            return _lastTokenResult;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _semaphore?.Dispose();
        }

        private ITokenClient GetClient()
        {
            if (_tokenClient != null)
            {
                return _tokenClient;
            }

            try
            {
                _tokenClient = _tokenClientFactory.CreateClient(_tokenManagerOptions.Client);

                return _tokenClient;
            }
            catch (TokenManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogEntry(LogLevel.Error, ex.Message, ex);

                throw new TokenManagerException(ex.Message, ex);
            }
        }

        private async Task<TokenResult> GetTokenAsync(bool forceRefresh = false)
        {
            if (LastTokenValid && !forceRefresh)
            {
                return _lastTokenResult;
            }

            // Ensure that only one request to refresh the access token is executed at any point in time.
            await _semaphore.WaitAsync();

            try
            {
                if (LastTokenValid && !forceRefresh)
                {
                    return _lastTokenResult;
                }

                var client = GetClient();
                _lastTokenResult = await client.LoginAsync(_tokenManagerOptions.Username, _tokenManagerOptions.Password);

                return _lastTokenResult;
            }
            //catch (UnauthorizedException)
            //{
            //    throw;
            //}
            catch (TokenManagerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogEntry(LogLevel.Error, ex.Message, ex);

                throw new TokenManagerException(ex.Message);
            }
            finally
            {
                // Release the semaphore lock for subsequent calls to the RefreshToken method.
                _semaphore.Release();
            }
        }

        private async Task<TokenResult> RefreshTokenAsync(bool forceRefresh = false)
        {
            var tokenResult = await GetTokenAsync(forceRefresh);

            // Set the token refresh timer
            var refreshIn = TimeSpan.FromSeconds(tokenResult.ExpiresIn / 2);
            var period = TimeSpan.FromMinutes(1);

            _timer?.Dispose();
            _timer = new Timer(async state => await RefreshTokenAsync(true), this, refreshIn, period);

            return tokenResult;
        }

        private void LogEntry(LogLevel logLevel, string message, Exception ex = null)
        {
            _logger.Log(logLevel, ex, $"{message} - {{details}}",
                new {_tokenManagerOptions.Client.AuthorityUrl, _tokenManagerOptions.Client.ClientId, _tokenManagerOptions.Client.Scopes, _tokenManagerOptions.Username});
        }
    }
}