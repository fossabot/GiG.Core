namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public abstract class CorrelationIdClusterLifetime : ClusterLifetime
    {
        protected CorrelationIdClusterLifetime() : base("Orleans:CorrelationIdSilo") { }
    }
}
