namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class ConsulMembershipProviderLifetime : MembershipProviderLifetime
    {
        public ConsulMembershipProviderLifetime() : base("Orleans:ConsulMembershipProvider", "Orleans:ConsulMemberShip", "Orleans:ConsulMemberShip:Silo")
        { }
    }
}