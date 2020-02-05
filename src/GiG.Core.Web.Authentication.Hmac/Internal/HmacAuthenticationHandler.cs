using GiG.Core.Authentication.Hmac.Abstractions;
using GiG.Core.Security.Cryptography;
using GiG.Core.Web.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("GiG.Core.Web.Security.Hmac.Tests.Unit")]
namespace GiG.Core.Web.Authentication.Hmac.Internal
{
    /// <summary>
    /// <see cref="HmacAuthenticationHandler"/> for hmac authentication header.
    /// </summary>
    internal class HmacAuthenticationHandler : AuthenticationHandler<HmacRequirement>
    {
        private readonly IHmacOptionsProvider _hmacOptionsProvider;
        private readonly IHashProviderFactory _signatureProviderFactory;
        private readonly IHmacSignatureProvider _hmacSignatureProvider;
        private readonly ILogger<HmacAuthenticationHandler> _logger;

        /// <summary>
        /// <see cref="AuthenticationHandler{TOptions}"/> using Hmac.
        /// </summary>
        public HmacAuthenticationHandler(
            IOptionsMonitor<HmacRequirement> optionsMonitor,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHmacOptionsProvider hmacOptionsProvider,
            IHashProviderFactory signatureProviderFactory,
            IHmacSignatureProvider hmacSignatureProvider)
            : base(optionsMonitor, logger, encoder, clock)
        {
            _hmacOptionsProvider = hmacOptionsProvider;
            _signatureProviderFactory = signatureProviderFactory;
            _hmacSignatureProvider = hmacSignatureProvider;
            _logger = logger.CreateLogger<HmacAuthenticationHandler>();
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var hmacOptions = _hmacOptionsProvider.GetHmacOptions();

            if (hmacOptions == null)
            {
                return AuthenticateResult.Fail("Hmac configuration not set.");
            }

            if (!Request.Headers.TryGetValue(Headers.Nonce,out var nonceValue))
            {
                return AuthenticateResult.Fail($"{Headers.Nonce} not set.");
            }

            Request.Headers.TryGetValue(Headers.Authorization, out var headerSignature);

            if (string.IsNullOrEmpty(headerSignature))
            {
                return AuthenticateResult.Fail("Hmac header is empty.");
            }

            var body = await Request.GetBodyAsync();
            var clearSignature = _hmacSignatureProvider.GetSignature(Request.Method.ToUpper(), Request.Path.Value, body, nonceValue, hmacOptions.Secret);
            var hashProvider = _signatureProviderFactory.GetHashProvider(hmacOptions.HashAlgorithm);
            var signature = hashProvider.Hash(clearSignature);
            var authHeader = AuthenticationHeaderValue.Parse(headerSignature);

            if (!signature.Equals(authHeader.Parameter))
            {
                _logger.LogDebug("Signature {signature}",clearSignature);
                return AuthenticateResult.Fail("Hmac does not match.");
            }

            var identity = new ClaimsIdentity(Scheme.Name); // the name of our auth scheme
            identity.AddClaim(new Claim("HMAC", authHeader.Parameter));

            var authTicket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                Scheme.Name);

            return AuthenticateResult.Success(authTicket);
        }
    }
}
