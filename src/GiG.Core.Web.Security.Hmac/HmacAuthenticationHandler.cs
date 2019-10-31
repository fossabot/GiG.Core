﻿using GiG.Core.Security.Cryptography;
using GiG.Core.Web.Security.Hmac.Abstractions;
using GiG.Core.Web.Security.Hmac.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GiG.Core.Web.Security.Hmac
{
    public class HmacAuthenticationHandler : AuthenticationHandler<HmacRequirement>
    {
        private readonly IHmacOptionsProvider _hmacOptionsProvider;
        private readonly IHashProviderFactory _signatureProviderFactory;
        private const string AuthHeader = "Authorization";
        private const string NonceHeader = "X-Nonce";
        public HmacAuthenticationHandler(
            IOptionsMonitor<HmacRequirement> optionsMonitor, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock, 
            IHmacOptionsProvider hmacOptionsProvider,
            IHashProviderFactory signatureProviderFactory)
            : base(optionsMonitor,logger,encoder,clock)
        {
            _hmacOptionsProvider = hmacOptionsProvider;
            _signatureProviderFactory = signatureProviderFactory;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var hmacOptions = _hmacOptionsProvider.GetHmacOptions();
            var hashProvider = _signatureProviderFactory.GetHashProvider(hmacOptions.HashAlgorithm);
            var signature = hashProvider.Hash(await Request.AsSignatureStringAsync(NonceHeader,hmacOptions.Secret));
            Request.Headers.TryGetValue(AuthHeader, out var headerSignature);

            if (string.IsNullOrEmpty(headerSignature))
            {
                
                return AuthenticateResult.Fail("Hmac does not match");
            }

            var authHeader = AuthenticationHeaderValue.Parse(headerSignature);

            if (!signature.Equals(authHeader.Parameter))
            {
                return AuthenticateResult.Fail("Hmac does not match");
            }

            var identity = new ClaimsIdentity(Scheme.Name); // the name of our auth scheme
            identity.AddClaim(new Claim("HMAC", authHeader.Parameter));
            
            var authTicket = new AuthenticationTicket(
                new System.Security.Claims.ClaimsPrincipal(identity),
                Scheme.Name);

            return AuthenticateResult.Success(authTicket);
        }
    }
}
