using GiG.Core.Orleans.Silo.Abstractions;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public abstract class DefaultClusterLifetime : ClusterLifetime
    {
        protected DefaultClusterLifetime() : base(SiloOptions.DefaultSectionName) { }
    }
}