using GiG.Core.Orleans.Clustering.Abstractions;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class ConsulMembershipProviderFixture : MembershipProviderFixture
    {
        public ConsulMembershipProviderFixture() : base(MembershipProviderOptions.DefaultSectionName)
        { }
    }
}