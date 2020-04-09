using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Docs.Authentication.ApiKey.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GiG.Core.Web.Docs.Authentication.ApiKey
{
    internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly ApiKeyOptions _apiKeyOptions;

        public ConfigureSwaggerOptions(IOptions<ApiKeyOptions> apiKeyAuthenticationOptionsAccessor)
        {
            _apiKeyOptions = apiKeyAuthenticationOptionsAccessor?.Value;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.IncludeApiKeyAuthentication(_apiKeyOptions);
        }
    }
}