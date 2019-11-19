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
        internal const string InvariantNamePostgreSQL = "Npgsql";

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The<see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
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
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfiguration configuration, [NotNull] string sectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(storageName)) throw new ArgumentException($"Missing {nameof(storageName)}.");
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException($"Missing {nameof(sectionName)}.");

            var namedStorageSectionName = $"{sectionName}:{storageName}";
            var storageConfigSection = configuration.GetSection(namedStorageSectionName);

            return builder.AddNpgsqlGrainStorage(storageName, storageConfigSection);
        }


        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));
            if (string.IsNullOrWhiteSpace(storageName)) throw new ArgumentException($"Missing {nameof(storageName)}.");

            var npgsqlOptions = configurationSection.Get<NpgsqlOptions>();

            if (npgsqlOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is not valid.");
            }

            builder.AddAdoNetGrainStorage(storageName, x =>
            {
                x.ConnectionString = npgsqlOptions.ConnectionString;
                x.Invariant = InvariantNamePostgreSQL;
                x.UseJsonFormat = true;
            });

            return builder;
        }
    }
}