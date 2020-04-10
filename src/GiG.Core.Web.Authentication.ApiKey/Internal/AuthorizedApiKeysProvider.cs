using GiG.Core.Authentication.ApiKey.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiG.Core.Web.Authentication.ApiKey.Abstractions;

namespace GiG.Core.Web.Authentication.ApiKey.Internal
{
    /// <summary>
    /// Default <see cref="ApiKeyOptions"/> provider for <see cref="ApiKeyAuthenticationHandler"/>.
    /// </summary>
    internal class AuthorizedApiKeysProvider : IAuthorizedApiKeysProvider
    {
        private readonly IOptionsMonitor<ApiKeyOptions> _apiKeyOptionsAccessor;

        /// <summary>
        /// Default <see cref="ApiKeyOptions"/> provider for <see cref="ApiKeyAuthenticationHandler"/>.
        /// </summary>
        /// <param name="apiKeyOptionsAccessor"><see cref="IOptionsMonitor{ApiKeyOptions}"/> used to retrieve the list of authorized keys from configuration.</param>
        public AuthorizedApiKeysProvider(IOptionsMonitor<ApiKeyOptions> apiKeyOptionsAccessor)
        {
            _apiKeyOptionsAccessor = apiKeyOptionsAccessor;
        }

        /// <summary>
        /// Returns the authorized valid api keys from configuration.
        /// </summary>
        /// <returns>Mapping between Api Key and Tenant Id.</returns>
        public Task<Dictionary<string,string>> GetAuthorizedApiKeysAsync()
        {
            return Task.FromResult(_apiKeyOptionsAccessor.CurrentValue.AuthorizedTenantKeys);
        }
    }
}