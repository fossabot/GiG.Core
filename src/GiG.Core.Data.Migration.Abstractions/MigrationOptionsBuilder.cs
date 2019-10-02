using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GiG.Core.Data.Migration.Abstractions
{
    /// <summary>
    /// Migration Options Builder.
    /// </summary>
    public class MigrationOptionsBuilder
    {
        private const string DefaultScriptsLocation = "Scripts";

        private readonly MigrationOptions _options;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="dbConnection">The <see cref="T:System.Data.IDbConnection" /> to be used for the DataBase migration.</param>
        private MigrationOptionsBuilder([NotNull] IServiceCollection services, [NotNull] IDbConnection dbConnection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            _options = new MigrationOptions(dbConnection);
            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Creates a MigrationOptionsBuilder.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="dbConnection">The <see cref="T:System.Data.IDbConnection" /> to be used for the DataBase migration.</param>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public static MigrationOptionsBuilder CreateMigrationBuilder(IServiceCollection services,
            IDbConnection dbConnection)
        {
            return new MigrationOptionsBuilder(services, dbConnection);
        }

        /// <summary>
        /// Sets the Default Migration Options.
        /// Default Script locations are \Scripts and \Scripts.{EnvironmentName}
        /// </summary>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public MigrationOptionsBuilder AddDefaultMigrationOptions()
        {
            var currentEnvironment = _serviceProvider.GetRequiredService<IHostEnvironment>();

            AddLocation(DefaultScriptsLocation);
            AddLocation($"{DefaultScriptsLocation}.{currentEnvironment.EnvironmentName}");

            return this;
        }

        /// <summary>
        /// Adds a Location for Scripts to the Migration Options.
        /// </summary>
        /// <param name="location">Scripts Location.</param>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public MigrationOptionsBuilder AddLocation(string location)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if (string.IsNullOrEmpty(basePath))
            {
                throw new ApplicationException("Assembly directory location cannot be NULL or empty");
            }

            _options.Locations.Add(Path.Combine(basePath, location));

            return this;
        }

        /// <summary>
        /// Adds a Location for Scripts to the Migration Options for the specified Environments.
        /// </summary>
        /// <param name="location">Scripts Location.</param>
        /// <param name="environments">List of Environments</param>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public MigrationOptionsBuilder AddLocation(string location, params string[] environments)
        {
            var currentEnvironment = _serviceProvider.GetRequiredService<IHostEnvironment>().EnvironmentName;

            if (environments.Any(x => x.Equals(currentEnvironment)))
            {
                AddLocation(location);
            }

            return this;
        }

        /// <summary>
        /// Sets the MetaData Table Schema Name.
        /// </summary>
        /// <param name="metadataTableSchema">The Metadata Table Schema Name.</param>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public MigrationOptionsBuilder AddMetadataTableSchema(string metadataTableSchema)
        {
            _options.MetadataTableSchema = metadataTableSchema;

            return this;
        }

        /// <summary>
        /// Sets the MetaData Table Name.
        /// </summary>
        /// <param name="metadataTableName">The Metadata Table Name.</param>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public MigrationOptionsBuilder AddMetadataTableName(string metadataTableName)
        {
            _options.MetadataTableName = metadataTableName;

            return this;
        }

        /// <summary>
        /// Disables the Migration.
        /// </summary>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolve.MigrationOptionsBuilder" />.</returns>
        public MigrationOptionsBuilder DisableMigration()
        {
            _options.Enabled = false;

            return this;
        }

        /// <summary>
        /// Perform the DataBase Migration.
        /// </summary>
        public void Migrate()
        {
            if (_options.Enabled && _options.Locations.Any())
            {
                _serviceProvider
                    .GetRequiredService<IDatabaseMigration>()
                    .Migrate(_options);
            }
        }
    }
}