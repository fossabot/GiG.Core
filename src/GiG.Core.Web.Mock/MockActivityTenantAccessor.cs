using GiG.Core.MultiTenant.Abstractions;
using System.Collections.Immutable;

namespace GiG.Core.Web.Mock
{
    /// <inheritdoc />
    public class MockActivityTenantAccessor : IActivityTenantAccessor
    {
        /// <inheritdoc />
        public IImmutableSet<string> Values => ImmutableHashSet.Create("1", "2");
    }
}