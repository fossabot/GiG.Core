namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class KubernetesMembershipProviderFixture : MembershipProviderFixture
    {
        public KubernetesMembershipProviderFixture() : base("Orleans:KubernetesMembershipProvider")
        { }
    }
}