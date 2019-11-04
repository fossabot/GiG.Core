using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Storage.Npgsql.Abstractions;
using GiG.Core.Orleans.Storage.Npgsql.Extensions;
using GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Fixtures
{
    public class ClusterFixture
    {
        internal readonly IClusterClient ClusterClient;

        internal readonly IServiceProvider ClientServiceProvider;

        internal readonly string CustomSectionName = "CustomStorageSection";

        public string ConnectionString { get; }
        public string CustomSectionConnectionString { get; }

        public ClusterFixture()
        {
            var siloHost = Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.ConfigureEndpoints();
                    x.UseLocalhostClustering();
                    x.AddAssemblies(typeof(StorageTestGrain));
                    x.AddNpgsqlGrainStorage("testDB", ctx.Configuration);
                    x.AddNpgsqlGrainStorage("testDB2", ctx.Configuration, CustomSectionName);
                })
                .Build();

            var configuration = siloHost.Services.GetRequiredService<IConfiguration>();
            ConnectionString = configuration.GetSection($"{NpgsqlOptions.DefaultSectionName}:testDB").Get<NpgsqlOptions>().ConnectionString;
            CustomSectionConnectionString = configuration.GetSection($"{CustomSectionName}:testDB2").Get<NpgsqlOptions>().ConnectionString;
            siloHost.StartAsync().GetAwaiter().GetResult();

            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.UseLocalhostClustering();
                        x.AddAssemblies(typeof(StorageTestGrain));
                    });
                })
                .Build();

            ClientServiceProvider = clientHost.Services;

            ClusterClient = ClientServiceProvider.GetRequiredService<IClusterClient>();
        }
    }
}