using GiG.Core.Authentication.ApiKey.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GiG.Core.Web.Authentication.ApiKey.Internal
{
    /// <summary>
    /// <see cref="AuthenticationHandler{T}"/> for header>
    /// </summary>
    internal class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IAuthorizedApiKeysProvider _apiKeysProvider;

        /// <summary>
        /// <see cref="AuthenticationHandler{TOptions}"/> using <see cref="Headers.ApiKey"/> header.
        /// </summary>
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> authenticationOptions,
            IAuthorizedApiKeysProvider apiKeysProvider,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) 
            : base(authenticationOptions, logger, encoder, clock)
        {
            _apiKeysProvider = apiKeysProvider;
        }

        /// <inheritdoc/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // If the X-Api-Key header is not present, let other handlers authenticate the request
            if (!Request.Headers.TryGetValue(Headers.ApiKey, out var apiKeyHeaderValue))
            {
                return AuthenticateResult.NoResult();
            }

            var authorizedTenantKeys = await _apiKeysProvider.GetAuthorizedApiKeysAsync();

            // If the Api Key in the header is not in the list of authorized keys, fail the authentication
            if (!authorizedTenantKeys.TryGetValue(apiKeyHeaderValue, out var tentantId))
            {
                return AuthenticateResult.Fail("Invalid API key.");
            }

            // If successful, create an authentication with the tenant id 

            var claims = new[] { new Claim("tenant_id", tentantId) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
