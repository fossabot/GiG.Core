﻿using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Messaging;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using GiG.Core.Orleans.Sample.Grains;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Constants = GiG.Core.Orleans.Sample.Contracts.Constants;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Sample.Silo
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMessagePublisher<WalletTransaction>, MessagePublisher<WalletTransaction>>();
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
                .AddMemoryGrainStorageAsDefault()
                .AddAssemblies(typeof(WalletGrain))
                .AddSimpleMessageStreamProvider(Constants.StreamProviderName)
                .UseSignalR()
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName);               
        }
    }
}