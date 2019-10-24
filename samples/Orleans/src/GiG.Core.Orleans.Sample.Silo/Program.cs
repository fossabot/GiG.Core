using GiG.Core.Configuration.Extensions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.Data.Migration.Evolve.Extensions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Storage.Npgsql;
using GiG.Core.Orleans.Storage.Npgsql.Abstractions;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace GiG.Core.Orleans.Sample.Silo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseApplicationMetadata()
                .ConfigureServices((ctx, services) => {
                    services.AddCorrelationAccessor();
                    services.AddRequestContextAccessor();
                    services.AddDbMigration(
                        new NpgsqlConnection(
                            ctx.Configuration[$"{NpgsqlOptions.DefaultSectionName}:{Constants.StorageProviderName}:ConnectionString"]))
                        .AddDefaultMigrationOptions()
                        .Migrate();
                })
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices)
                .UseOrleans(Startup.ConfigureOrleans);
    }
}