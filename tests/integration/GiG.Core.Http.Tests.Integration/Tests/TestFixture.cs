using GiG.Core.Http.Tests.Integration.Mocks;

namespace GiG.Core.Http.Tests.Integration.Tests
{
    public class TestFixture
    {
        internal readonly MockCorrelationContextAccessor CorrelationContextAccessor;
        internal readonly MockTenantAccessor TenantAccessor;

        public TestFixture()
        {
            CorrelationContextAccessor = new MockCorrelationContextAccessor();
            TenantAccessor = new MockTenantAccessor();
        }
    }
}