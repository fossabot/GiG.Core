using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.Web.Mock;

namespace GiG.Core.Http.Tests.Integration.Mocks
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