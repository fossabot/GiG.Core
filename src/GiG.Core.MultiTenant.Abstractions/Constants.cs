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

        public const string TenantIdBaggageKey = "TenantId";
    }
}