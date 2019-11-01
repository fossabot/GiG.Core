using GiG.Core.Security.Cryptography;
using GiG.Core.Security.Http;
using GiG.Core.Web.Security.Hmac.Abstractions;
using GiG.Core.Web.Security.Hmac.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GiG.Core.Web.Security.Hmac
{
    /// <summary>
    /// <see cref="HmacAuthenticationHandler"/> for hmac authentication header.
    /// </summary>
    public class HmacAuthenticationHandler : AuthenticationHandler<HmacRequirement>
    {
        private readonly IHmacOptionsProvider _hmacOptionsProvider;
        private readonly IHashProviderFactory _signatureProviderFactory;

        /// <summary>
        /// <see cref="AuthenticationHandler{TOptions}"/> using Hmac.
        /// </summary>
        public HmacAuthenticationHandler(
            IOptionsMonitor<HmacRequirement> optionsMonitor,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHmacOptionsProvider hmacOptionsProvider,
            IHashProviderFactory signatureProviderFactory)
            : base(optionsMonitor, logger, encoder, clock)
        {
            _hmacOptionsProvider = hmacOptionsProvider;
            _signatureProviderFactory = signatureProviderFactory;
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var hmacOptions = _hmacOptionsProvider.GetHmacOptions();

            if (hmacOptions == null)
            {
                return AuthenticateResult.Fail($"Hmac configuration not set.");
            }

            var hashProvider = _signatureProviderFactory.GetHashProvider(hmacOptions.HashAlgorithm);

            if (!Request.Headers.ContainsKey(HmacConstants.NonceHeader))
            {
                return AuthenticateResult.Fail($"{HmacConstants.NonceHeader} not set.");
            }

            var signature = hashProvider.Hash(await Request.AsSignatureStringAsync(HmacConstants.NonceHeader, hmacOptions.Secret));
            Request.Headers.TryGetValue(HmacConstants.AuthHeader, out var headerSignature);

            if (string.IsNullOrEmpty(headerSignature))
            {
                return AuthenticateResult.Fail("Hmac does not match.");
            }

            var authHeader = AuthenticationHeaderValue.Parse(headerSignature);

            if (!signature.Equals(authHeader.Parameter))
            {
                return AuthenticateResult.Fail("Hmac does not match.");
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
