using System.Collections.Immutable;

namespace GiG.Core.MultiTenant.Abstractions
{
    /// <summary>
    /// Activity Tenants Accessor.
    /// </summary>
    public interface IActivityTenantAccessor
    {
        /// <summary>
        /// Contains the current context's Tenants.
        /// </summary>
        IImmutableSet<string> Values { get; }
    }
}