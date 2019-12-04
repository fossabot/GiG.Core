namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class KubernetesMembershipProviderLifetime : MembershipProviderLifetime
    {
        public KubernetesMembershipProviderLifetime() : base("Orleans:KubernetesMembershipProvider", "Orleans:KubernetesMemberShip", "Orleans:KubernetesMemberShip:Silo")
        { }
    }
}