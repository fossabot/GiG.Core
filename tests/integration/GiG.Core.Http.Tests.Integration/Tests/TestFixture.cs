using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Http.Tests.Integration.Mocks;
using GiG.Core.MultiTenant.Abstractions;

namespace GiG.Core.Http.Tests.Integration.Tests
{
    public class TestFixture
    {
        internal readonly ICorrelationContextAccessor CorrelationContextAccessor;
        internal readonly ITenantAccessor TenantAccessor;

        public TestFixture()
        {
            CorrelationContextAccessor = new MockCorrelationContextAccessor();
            TenantAccessor = new MockTenantAccessor();
        }
    }
}