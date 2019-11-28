using GiG.Core.TokenManager.Abstractions.Interfaces;
using GiG.Core.TokenManager.Abstractions.Models;

namespace GiG.Core.TokenManager.Implementation
{
    internal class TokenManagerBuilder : ITokenManagerBuilder
    {
        internal TokenManagerOptions Config { get; }

        internal TokenManagerBuilder() => Config = new TokenManagerOptions();

        public ITokenManagerBuilder WithAuthorityUrl(string url)
        {
            Config.Client.AuthorityUrl = url;

            return this;
        }

        public ITokenManagerBuilder WithClientId(string clientId)
        {
            Config.Client.ClientId = clientId;
            
            return this;
        }

        public ITokenManagerBuilder WithClientSecret(string clientSecret)
        {
            Config.Client.ClientSecret = clientSecret;
            
            return this;
        }

        public ITokenManagerBuilder WithUsername(string username)
        {
            Config.Username = username;
            
            return this;
        }

        public ITokenManagerBuilder WithPassword(string password)
        {
            Config.Password = password;
            
            return this;
        }

        public ITokenManagerBuilder WithScopes(string scopes)
        {
            Config.Client.Scopes = scopes;
            
            return this;
        }

        public ITokenManagerBuilder WithRequireHttps(bool value)
        {
            Config.Client.RequireHttps = value;
            
            return this;
        }
    }
}
