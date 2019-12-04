namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public abstract class CorrelationIdClusterLifetime : ClusterLifetime
    {
        public CorrelationIdClusterLifetime() : base("Orleans:CorrelationIdSilo") { }
    }
}
