using System.Collections.Generic;
using System.Collections.Immutable;

namespace GiG.Core.MultiTenant.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITenantAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        IImmutableSet<string> Values { get; }
    }
}