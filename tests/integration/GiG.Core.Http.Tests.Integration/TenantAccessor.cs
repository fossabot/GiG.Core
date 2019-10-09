using GiG.Core.MultiTenant.Abstractions;
using System.Collections.Immutable;

namespace GiG.Core.Http.Tests.Integration
{
    internal class TenantAccessor : ITenantAccessor
    {
        public IImmutableSet<string> Values => ImmutableHashSet.Create("1", "2");
    }
}
