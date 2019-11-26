namespace GiG.Core.TokenManager.Interfaces
{
    public interface ITokenManagerBuilder
    {
        ITokenManagerBuilder WithAuthorityUrl(string url);

        ITokenManagerBuilder WithClientId(string clientId);

        ITokenManagerBuilder WithClientSecret(string clientSecret);

        ITokenManagerBuilder WithUsername(string username);

        ITokenManagerBuilder WithPassword(string password);

        ITokenManagerBuilder WithScopes(string scopes);

        ITokenManagerBuilder WithRequireHttps(bool value);
    }
}