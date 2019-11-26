using GiG.Core.TokenManager.Models;

namespace GiG.Core.TokenManager.Interfaces
{
    /// <summary>
    /// Provides a factory to create an instance of <see cref="ITokenClient"/>.
    /// </summary>
    public interface ITokenClientFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="ITokenClient"/>
        /// </summary>
        /// <param name="tokenClientOptions">The client configuration to be used by <see cref="ITokenClient"/></param>
        /// <returns>An instance of <see cref="ITokenClient"/></returns>
        ITokenClient CreateClient(TokenClientOptions tokenClientOptions);
    }
}