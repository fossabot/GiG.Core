namespace GiG.Core.TokenManager.Interfaces
{
    /// <summary>
    /// Token manager builder.
    /// </summary>
    public interface ITokenManagerBuilder
    {
        /// <summary>
        /// The Authority URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithAuthorityUrl(string url);

        /// <summary>
        /// The Client Id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithClientId(string clientId);

        /// <summary>
        /// The Client Secret.
        /// </summary>
        /// <param name="clientSecret"></param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithClientSecret(string clientSecret);

        /// <summary>
        /// The Username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithUsername(string username);

        /// <summary>
        /// The Password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithPassword(string password);

        /// <summary>
        /// The Scopes.
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithScopes(string scopes);

        /// <summary>
        /// Is Https required?
        /// </summary>
        /// <param name="value">True if Https is required, otherwise false.</param>
        /// <returns>The <see cref="ITokenManagerBuilder"/>.</returns>
        ITokenManagerBuilder WithRequireHttps(bool value);
    }
}