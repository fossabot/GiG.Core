using GiG.Core.Orleans.Silo.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class DefaultClusterFixture : ClusterLifetime
    {
        public DefaultClusterFixture() : base(SiloOptions.DefaultSectionName) { }
    }
}