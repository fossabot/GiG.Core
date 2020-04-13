using GiG.Core.Web.Docs.Authentication.ApiKey.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GiG.Core.Web.Docs.Authentication.ApiKey
{
    internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.IncludeApiKeyAuthentication();
        }
    }
}