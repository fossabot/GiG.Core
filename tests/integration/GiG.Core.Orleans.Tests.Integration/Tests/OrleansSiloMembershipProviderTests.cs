﻿using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
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
                    sb.ConfigureEndpoints(ctx.Configuration.GetSection("Orleans:ClusterA:Silo"));
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