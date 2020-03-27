﻿using Microsoft.AspNetCore.Authentication;

namespace GiG.Core.Web.Authentication.ApiKey.Internal
{
    internal class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "apikey";
    }
}