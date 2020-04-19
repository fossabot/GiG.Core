using System.Collections.Generic;

namespace GiG.Core.Web.Authentication.ApiKey.Abstractions
{
    /// <summary>
    /// Options for ApiKeyAuthentication.
    /// </summary>
    public class ApiKeyOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Authentication:ApiKey";

        /// <summary>
        /// Mapping between Api Keys and the associated Tenant Id.
        /// </summary>
        public IDictionary<string, string> AuthorizedTenantKeys { get; set; }
    }
}
