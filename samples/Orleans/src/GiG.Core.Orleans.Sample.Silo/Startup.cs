﻿using GiG.Core.Data.Migration.Evolve.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Grains;
using GiG.Core.Orleans.Storage.Npgsql.Configurations;
using GiG.Core.Orleans.Storage.Npgsql.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Sample.Silo
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure Orleans.
        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureDashboard(ctx.Configuration)
                .ConfigureEndpoints()
                .UseMembershipProvider(ctx.Configuration, x =>
                {
                    x.ConfigureConsulClustering(ctx.Configuration);
                    x.ConfigureKubernetesClustering(ctx.Configuration);
                })
                .AddNpgsqlGrainStorage("sampleDb", ctx.Configuration)
                .AddAssemblies(typeof(WalletGrain))
                .AddSimpleMessageStreamProvider(Constants.StreamProviderName)
                .UseSignalR()
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName);               
        }
    }
}