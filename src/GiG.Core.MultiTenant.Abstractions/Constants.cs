namespace GiG.Core.MultiTenant.Abstractions
{
    /// <summary>
    /// Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The Header for Tenant ID.
        /// </summary>
        public const string Header = "X-Tenant-ID";

        /// <summary>
        /// Baggage Key for Tenant ID.
        /// </summary>
        public const string TenantIdBaggageKey = "TenantId";
        
        /// <summary>
        /// Claim Type for Tenant ID.
        /// </summary>
        public const string ClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
    }
}