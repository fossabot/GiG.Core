using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Web.Authentication.ApiKey.Internal
{
    internal class DefaultAuthorizedApiKeysProvider : IAuthorizedApiKeysProvider
    {
        private readonly IOptionsMonitor<ApiKeyOptions> _apiKeyOptions;

        public DefaultAuthorizedApiKeysProvider(IOptionsMonitor<ApiKeyOptions> apiKeyOptions) {
            _apiKeyOptions = apiKeyOptions;
        }

        public Task<Dictionary<string,string>> GetAuthorizedApiKeysAsync()
        {
            return Task.FromResult(_apiKeyOptions.CurrentValue.AuthorizedTenantKeys);
        }
    }
}
