using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Silo.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Client.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Silo.Extensions;
using GiG.Core.Orleans.Clustering.Silo.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansClientMembershipProviderTests : AbstractMembershipProviderTests
    {       
        public OrleansClientMembershipProviderTests()
        {
            _hostBuilder = Host.CreateDefaultBuilder()
             .ConfigureServices((ctx, services) =>
             {
                 services.AddClusterClient(x =>
                 {
                     x.ConfigureCluster(ctx.Configuration);
                     x.UseMembershipProvider(ctx.Configuration, builder =>
                     {
                         builder.ConfigureConsulClustering(ctx.Configuration);
                         builder.ConfigureKubernetesClustering(ctx.Configuration);
                     });
                     x.AddAssemblies(typeof(IEchoTestGrain));
                 });
             });
        }
    }
}
