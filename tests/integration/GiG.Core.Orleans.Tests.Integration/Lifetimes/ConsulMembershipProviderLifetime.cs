using GiG.Core.Orleans.Clustering.Abstractions;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class ConsulMembershipProviderLifetime : MembershipProviderLifetime
    {
        public ConsulMembershipProviderLifetime() : base(MembershipProviderOptions.DefaultSectionName, "Orleans:ConsulMemberShip", "Orleans:ConsulMemberShip:Silo")
        { }
    }
}