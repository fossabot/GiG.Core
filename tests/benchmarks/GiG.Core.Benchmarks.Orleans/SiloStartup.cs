using GiG.Core.Benchmarks.Orleans.StorageProviders;
using GiG.Core.Benchmarks.Orleans.Streams.Grains;
using GiG.Core.Orleans.Silo.Dashboard.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Storage.Npgsql.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;
using System.Collections.Generic;
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
                .UseMongoDBClient(GetConnectionString(ctx.Configuration, StorageProvidersConstants.MongoDb))
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
                .AddDynamoDBGrainStorage(StorageProvidersConstants.DynamoDb, options =>
                {
                    options.UseJson = true;
                    options.Service = GetConnectionString(ctx.Configuration, StorageProvidersConstants.DynamoDb);
                })
                .AddNpgsqlGrainStorage(StorageProvidersConstants.Postgres, ctx.Configuration)
                .AddKafka(Constants.KafkaProviderName)
                .WithOptions(options =>
                {
                    options.FromConfiguration(ctx.Configuration);
                    options.ConsumeMode = ConsumeMode.StreamEnd;

                    options.AddTopic(Constants.MessageNamespace);
                })
                .AddJson()
                .Build()
                .AddRedisGrainStorage(StorageProvidersConstants.Redis)
                .Build(config => config.Configure(opts =>
                    {
                        opts.Servers = new List<string> {GetConnectionString(ctx.Configuration, StorageProvidersConstants.Redis)};
                        opts.ClientName = StorageProvidersConstants.Redis;
                        opts.KeyPrefix = "OrleansGrainStorage";
                        opts.HumanReadableSerialization = true;
                    })
                );
        }
        
        private static string GetConnectionString(IConfiguration configuration, string provider)
        {
            var configSection = configuration.GetSection($"Orleans:StorageProviders:{provider}:ConnectionString");

            return configSection.Value;
        }
    }
}