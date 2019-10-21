using GiG.Core.Data.Migration.Abstractions;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Data.Migration.Evolve.Internal
{
    internal class DatabaseMigration : IDatabaseMigration
    {
        private readonly ILogger _logger;

        public DatabaseMigration(ILogger<DatabaseMigration> logger)
        {
            _logger = logger;
        }

        public void Migrate(MigrationOptions options)
        {
            if (options.IsEnabled)
            {
                var evolve = new global::Evolve.Evolve(options.Connection, x => _logger.LogDebug(x))
                {
                    Locations = options.Locations,
                    MetadataTableSchema = options.MetadataTableSchema,
                    MetadataTableName = options.MetadataTableName
                };

                evolve.Migrate();
            }
        }
    }
}