using System.Collections.Immutable;

namespace GiG.Core.MultiTenant.Abstractions
{
    /// <summary>
    /// Tenants Accessor.
    /// </summary>
    public interface ITenantAccessor
    {
        /// <summary>
        /// Contains the current context's Tenants.
        /// </summary>
        IImmutableSet<string> Values { get; }
    }
}