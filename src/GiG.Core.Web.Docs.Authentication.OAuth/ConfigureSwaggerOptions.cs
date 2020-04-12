using GiG.Core.Web.Authentication.OAuth.Abstractions;
using GiG.Core.Web.Docs.Authentication.OAuth.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace GiG.Core.Web.Docs.Authentication.OAuth
{
    internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly OAuthAuthenticationOptions _oAuthAuthenticationOptions;

        public ConfigureSwaggerOptions(IOptions<OAuthAuthenticationOptions> oAuthAuthenticationOptionsAccessor)
        {
            if (oAuthAuthenticationOptionsAccessor.Value?.Authority == null)
            {
                throw new ApplicationException("The following service is missing; Add services.ConfigureOAuthAuthentication(...)");
            }
            
            _oAuthAuthenticationOptions = oAuthAuthenticationOptionsAccessor.Value;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.IncludeOAuthAuthentication(_oAuthAuthenticationOptions);
        }
    }
}