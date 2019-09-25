using GiG.Core.Orleans.Clustering.Consul.Silo.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Silo.Extensions;
using GiG.Core.Orleans.Clustering.Silo.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Grains;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansSiloMembershipProviderTests : AbstractMembershipProviderTests
    {       
        public OrleansSiloMembershipProviderTests()
        {
            HostBuilder = Host.CreateDefaultBuilder()
                 .UseOrleans((ctx, sb) =>
                 {
                     sb.ConfigureCluster(ctx.Configuration);
                     sb.ConfigureEndpoints();
                     sb.UseMembershipProvider(ctx.Configuration, x =>
                     {
                         x.ConfigureConsulClustering(ctx.Configuration);
                         x.ConfigureKubernetesClustering(ctx.Configuration);
                     });
                     sb.AddAssemblies(typeof(EchoTestGrain));
                 });
        } 
    }
}
