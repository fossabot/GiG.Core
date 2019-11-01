using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.Web.Security.Hmac.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Web.Security.Hmac.MultiTenant
{
    /// <summary>
    /// <see cref="MultiTenantOptionProvider"/> for HmacAuthenticationHandler.
    /// </summary>
    internal class MultiTenantOptionProvider : IHmacOptionsProvider
    {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IOptions<Dictionary<string, HmacOptions>> _optionAccessor;
        private readonly ILogger<MultiTenantOptionProvider> _logger;

        /// <summary>
        /// <see cref="IHmacOptionsProvider"/> which handles multi-tenancy for HmacAuthenticationHandler.
        /// </summary>
        public MultiTenantOptionProvider(
            ITenantAccessor tenantAccessor,
            IOptionsSnapshot<Dictionary<string, HmacOptions>> optionsAccessor,
            ILogger<MultiTenantOptionProvider> logger)
        {
            _tenantAccessor = tenantAccessor;
            _optionAccessor = optionsAccessor;
            _logger = logger;
        }
        /// <summary>
        /// Get <see cref="HmacOptions"/> for multi-tenancy. This has a limitation that it only accepts the first tenant in the HttpHeaders.
        /// </summary>
        /// <returns>Options for the current tenant.</returns>
        public HmacOptions GetHmacOptions()
        {
            var tenantId = _tenantAccessor.Values.FirstOrDefault();
            
            if (tenantId == null)
            {
                _logger.LogWarning("No tenantId found.");
                return null;
            }
            var options = _optionAccessor.Value;
            if(!options.TryGetValue(tenantId, out var tenantOptions))
            {
                _logger.LogWarning("No config found for {@tenantId}.",tenantId);
                return null;
            }
            return tenantOptions;
        }
    }
}
