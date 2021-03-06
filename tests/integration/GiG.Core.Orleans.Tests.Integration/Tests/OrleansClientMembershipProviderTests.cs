﻿using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
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
            HostBuilder = Host.CreateDefaultBuilder()
             .ConfigureServices((ctx, services) =>
             {
                 services.AddDefaultClusterClient(x =>
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
