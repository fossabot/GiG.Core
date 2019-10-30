using GiG.Core.Orleans.Storage.Npgsql.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Storage.Npgsql.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        internal const string InvariantNamePostgreSql = "Npgsql";

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The<see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, string storageName, [NotNull] IConfiguration configuration)
        {
            return builder.AddNpgsqlGrainStorage(storageName, configuration, NpgsqlOptions.DefaultSectionName);
        }

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="sectionName">The Storage Providers section name.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, string storageName, [NotNull] IConfiguration configuration, string sectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var namedStoreageSectionName = $"{sectionName}:{storageName}";
            var storageConfigSection = configuration.GetSection(namedStoreageSectionName);

            return builder.AddNpgsqlGrainStorage(storageName, storageConfigSection);
        }


        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, string storageName, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            var npgsqlOptions = configurationSection.Get<NpgsqlOptions>();

            if (npgsqlOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is not valid.");
            }

            builder.AddAdoNetGrainStorage(storageName, x =>
            {
                x.ConnectionString = npgsqlOptions.ConnectionString;
                x.Invariant = InvariantNamePostgreSql;
                x.UseJsonFormat = true;
            });

            return builder;
        }
    }
}