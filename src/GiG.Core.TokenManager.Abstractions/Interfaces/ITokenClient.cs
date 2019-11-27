using GiG.Core.TokenManager.Abstractions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.TokenManager.Abstractions.Interfaces
{
    /// <summary>
    /// Token client used to retrieve an access or refresh token, provided the correct credentials are passed.
    /// </summary>
    public interface ITokenClient : IDisposable
    {
        /// <summary>
        /// Calls discovery endpoint to retrieve metadata on identity server
        /// </summary>
        /// <returns>Discovery metadata including token endpoint</returns>
        Task<DiscoveryResult> GetDiscoveryAsync();
        
        /// <summary>
        /// Requests a token using the resource owner password credentials
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <param name="password">The password</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Token result including Access Token</returns>
        Task<TokenResult> LoginAsync(string userName, string password,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Requests a token using the resource owner password credentials
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <param name="password">The password</param>
        /// <param name="scopes">Client scopes</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Token result including Access Token</returns>
        Task<TokenResult> LoginAsync(string userName, string password, string scopes,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Requests a token using a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Token result including Access Token</returns>
        Task<TokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}