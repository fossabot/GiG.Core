using GiG.Core.Data.Migration.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GiG.Core.Data.Migration
{
    public class MigrationOptionsBuilder
    {
        private const string DefaultScriptsLocation = "Scripts";

        private readonly MigrationOptions _options;
        private readonly IServiceProvider _serviceProvider;

        MigrationOptionsBuilder(IServiceCollection services, IDbConnection dbConnection)
        {
            _serviceProvider = services?.BuildServiceProvider();
            _options = new MigrationOptions(dbConnection);
        }

        public static MigrationOptionsBuilder CreateMigrationBuilder(IServiceCollection services, IDbConnection dbConnection)
        {
            return new MigrationOptionsBuilder(services, dbConnection);
        }

        public MigrationOptionsBuilder AddDefaultMigrationOptions()
        {
            var env = _serviceProvider.GetRequiredService<IHostingEnvironment>();

            AddLocation(DefaultScriptsLocation);
            AddLocation($"{DefaultScriptsLocation}.{env.EnvironmentName}");

            return this;
        }

        public MigrationOptionsBuilder AddLocation(string location)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _options.Locations.Add(Path.Combine(basePath, location));

            return this;
        }

        public MigrationOptionsBuilder AddLocation(string location, params string[] environments)
        {
            var env = _serviceProvider.GetRequiredService<IHostingEnvironment>().EnvironmentName;
            environments.Where(x => x.Equals(env))?.ToList().ForEach(e => AddLocation(location));

            return this;
        }

        public MigrationOptionsBuilder AddMetadataTableSchema(string metadataTableSchema)
        {
            _options.MetadataTableSchema = metadataTableSchema;

            return this;
        }

        public MigrationOptionsBuilder AddMetadataTableName(string metadataTableName)
        {
            _options.MetadataTableName = metadataTableName;

            return this;
        }

        public MigrationOptionsBuilder DisableMigration()
        {
            _options.Enabled = false;

            return this;
        }

        public void Migrate()
        {
            if (_options.Enabled && _options.Locations.Any())
            {
                var dbUtils = _serviceProvider.GetRequiredService<DatabaseMigration>();
                dbUtils.Migrate(_options);
            }
        }
    }
}
