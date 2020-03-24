using GiG.Core.Authentication.ApiKey.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Web.Authentication.ApiKey.Internal
{
    /// <summary>
    /// Default <see cref="ApiKeyOptions"/> provider for <see cref="ApiKeyAuthenticationHandler"/>.
    /// </summary>
    internal class AuthorizedApiKeysProvider : IAuthorizedApiKeysProvider
    {
        private readonly IOptionsMonitor<ApiKeyOptions> _apiKeyOptions;

        /// <summary>
        /// Default <see cref="ApiKeyOptions"/> provider for <see cref="ApiKeyAuthenticationHandler"/>.
        /// </summary>
        /// <param name="apiKeyOptions"><see cref="IOptionsMonitor{ApiKeyOptions}"/> used to retrieve the list of authorized keys from configuration.</param>
        public AuthorizedApiKeysProvider(IOptionsMonitor<ApiKeyOptions> apiKeyOptions) {
            _apiKeyOptions = apiKeyOptions;
        }

        /// <summary>
        /// Returns the authorized valid api keys from configuration.
        /// </summary>
        /// <returns>Mapping between Api Key and Tenant Id.</returns>
        public Task<Dictionary<string,string>> GetAuthorizedApiKeysAsync()
        {
            return Task.FromResult(_apiKeyOptions.CurrentValue.AuthorizedTenantKeys);
        }
    }
}