using GiG.Core.TokenManager.Abstractions.Models;
using System;

namespace GiG.Core.TokenManager.Abstractions.Interfaces
{
    /// <summary>
    /// Provides a factory to create an instance of <see cref="ITokenManager"/>.
    /// </summary>
    public interface ITokenManagerFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="ITokenManager"/> using Fluent API configuration style.
        /// </summary>
        /// <param name="configurationActions">An action of type <see cref="ITokenManagerBuilder"/> with the required configuration.</param>
        /// <returns>An instance of <see cref="ITokenManager"/> based on the provided configuration.</returns>
        ITokenManager Create(Action<ITokenManagerBuilder> configurationActions = null);

        /// <summary>
        /// Creates an instance of <see cref="ITokenManager"/> using the provided configuration.
        /// </summary>
        /// <param name="options">The configuration options to be used by the <see cref="ITokenManager"/> instance.</param>
        /// <returns>An instance of <see cref="ITokenManager"/> based on the provided configuration.</returns>
        ITokenManager Create(TokenManagerOptions options);
    }
}