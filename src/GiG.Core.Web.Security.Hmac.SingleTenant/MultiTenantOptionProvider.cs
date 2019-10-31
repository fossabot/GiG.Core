using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.Web.Security.Hmac.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Web.Security.Hmac.MultiTenant
{
    public class MultiTenantOptionProvider : IHmacOptionsProvider
    {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IOptions<Dictionary<string, HmacOptions>> _optionAccessor;

        public MultiTenantOptionProvider(
            ITenantAccessor tenantAccessor,
            IOptionsSnapshot<Dictionary<string, HmacOptions>> optionsAccessor)
        {
            _tenantAccessor = tenantAccessor;
            _optionAccessor = optionsAccessor;
        }
        /// <summary>
        /// Get HMAC options for multitenancy. This has a limitation that it only accepts the first tenant in the HttpHeaders.
        /// </summary>
        /// <returns>Options for the current tenant.</returns>
        public HmacOptions GetHmacOptions()
        {
            var tenantId = _tenantAccessor.Values.FirstOrDefault();
            if (tenantId == null)
            {
                return null;
            }
            var options = _optionAccessor.Value;
            if(options.TryGetValue(tenantId, out var tenantOptions))
            {
                //throw exception
                return null;
            }
            return tenantOptions;
        }
    }
}
