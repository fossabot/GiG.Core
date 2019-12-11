﻿using GiG.Core.Benchmarks.Orleans.StorageProviders;
using GiG.Core.Benchmarks.Orleans.Streams.Grains;
using GiG.Core.Orleans.Silo.Dashboard.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Newtonsoft.Json;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;
using System.Data.Common;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Benchmarks.Orleans
{
    public static class SiloStartup
    {
        // This method gets called by the runtime. Use this method to configure Orleans.
        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureDashboard(ctx.Configuration)
                .ConfigureEndpoints(ctx.Configuration)
                .UseLocalhostClustering()
                .AddMemoryGrainStorageAsDefault()
                .AddAssemblies(typeof(ProducerGrain))
                .AddSimpleMessageStreamProvider(Constants.SMSProviderName)
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName)
                .AddMemoryGrainStorage(name: StorageProvidersConstants.InMemory)
                .UseMongoDBClient(ConnectionStrings.MongoDb)
                .AddMongoDBGrainStorage(StorageProvidersConstants.MongoDb, options =>
                {
                    options.DatabaseName = StorageProvidersConstants.DatabaseName;
                    options.CreateShardKeyForCosmos = true;
                    
                    options.ConfigureJsonSerializerSettings = settings =>
                    {
                        settings.NullValueHandling = NullValueHandling.Include;
                        settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                        settings.DefaultValueHandling = DefaultValueHandling.Populate;
                    };
                })
                .AddKafka(Constants.KafkaProviderName)
                .WithOptions(options =>
                {
                    options.FromConfiguration(ctx.Configuration);
                    options.ConsumeMode = ConsumeMode.StreamEnd;

                    options.AddTopic(Constants.MessageNamespace);
                })
                .AddJson()
                .Build();
        }
    }
}