using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("GiG.Core.Web.Authentication.ApiKey.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace GiG.Core.Web.Authentication.ApiKey.Internal
{
    /// <summary>
    /// <see cref="AuthenticationHandler{T}"/> for header.
    /// </summary>
    internal class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IAuthorizedApiKeysProvider _apiKeysProvider;
        private readonly ILogger<ApiKeyAuthenticationHandler> _logger;

        /// <summary>
        /// <see cref="AuthenticationHandler{TOptions}"/> using <see cref="Headers.ApiKey"/> header.
        /// </summary>
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> authenticationOptions,
            IAuthorizedApiKeysProvider apiKeysProvider,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock) 
            : base(authenticationOptions, loggerFactory, encoder, clock)
        {
            _logger = loggerFactory.CreateLogger<ApiKeyAuthenticationHandler>();
            _apiKeysProvider = apiKeysProvider;
        }

        /// <inheritdoc/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // If the X-Api-Key header is not present, let other handlers authenticate the request.
            if (!Request.Headers.TryGetValue(Headers.ApiKey, out var apiKeyHeaderValue))
            {
                return AuthenticateResult.NoResult();
            }

            // Check that the Api Key in the header has a value.
            if (string.IsNullOrWhiteSpace(apiKeyHeaderValue))
            {
                return FailedApiKeyAuthentication();
            }

            var authorizedApiKeys = await _apiKeysProvider.GetAuthorizedApiKeysAsync();

            // Validate the provided Authorized Api Keys.
            if (authorizedApiKeys == null ||
                !authorizedApiKeys.Any())
            {
                _logger.LogError("ApiKey Authentication was enabled but the provided list of Authorized Api Keys was empty.");
                return FailedApiKeyAuthentication();
            }

            // If the value of the Api Key in the header is not in the list of authorized keys, fail the authentication.
            if (!authorizedApiKeys.TryGetValue(apiKeyHeaderValue, out var tenantId) ||
                string.IsNullOrWhiteSpace(tenantId))
            {
                return FailedApiKeyAuthentication();
            }

            // If it is in the list, create an authentication ticket with the tenant id and succeed.
            Activity.Current?.AddBaggage(Constants.TenantIdBaggageKey, tenantId);

            var claims = new[] { new Claim(Constants.ClaimType, tenantId) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private AuthenticateResult FailedApiKeyAuthentication()
        {
            return AuthenticateResult.Fail("Invalid API key.");
        }
    }
}
