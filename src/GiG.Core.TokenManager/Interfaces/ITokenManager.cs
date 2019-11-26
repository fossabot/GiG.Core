using GiG.Core.TokenManager.Models;
using System;
using System.Threading.Tasks;

namespace GiG.Core.TokenManager.Interfaces
{
    /// <summary>
    /// Token manager used to retrieve a token.
    /// </summary>
    public interface ITokenManager : IDisposable
    {
        /// <summary>
        /// Requests a token using the resource owner password credentials
        /// </summary>
        /// <returns>Token result including Access Token</returns>
        Task<TokenResult> GetAndRefreshTokenAsync();
    }
}
