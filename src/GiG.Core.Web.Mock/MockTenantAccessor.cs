using GiG.Core.MultiTenant.Abstractions;
using System.Collections.Immutable;

namespace GiG.Core.Web.Mock
{
    public class MockTenantAccessor : ITenantAccessor
    {
        public IImmutableSet<string> Values => ImmutableHashSet.Create("1", "2");
    }
}