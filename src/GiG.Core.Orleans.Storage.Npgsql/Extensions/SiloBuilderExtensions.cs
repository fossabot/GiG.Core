using GiG.Core.Orleans.Storage.Npgsql.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using Orleans.Providers;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Storage.Npgsql.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        private const string InvariantNamePostgreSQL = "Npgsql";

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider using Default Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <returns>The<see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorageAsDefault([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return builder.AddNpgsqlGrainStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME, configuration);
        }

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <returns>The<see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return builder.AddNpgsqlGrainStorage(storageName, configuration, storageName);
        }

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <param name="storageSectionName">The Storage Provider section name.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfiguration configuration, [NotNull] string storageSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(storageName)) throw new ArgumentException($"'{nameof(storageName)}' must not be null, empty or whitespace.", nameof(storageName));
            if (string.IsNullOrWhiteSpace(storageSectionName)) throw new ArgumentException($"'{nameof(storageSectionName)}' must not be null, empty or whitespace.", nameof(storageSectionName));

            var namedStorageSectionName = $"{NpgsqlOptions.DefaultSectionName}:{storageSectionName}";
            var storageConfigSection = configuration.GetSection(namedStorageSectionName);

            return builder.AddNpgsqlGrainStorage(storageName, storageConfigSection);
        }


        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage name.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");
            if (string.IsNullOrWhiteSpace(storageName)) throw new ArgumentException($"'{nameof(storageName)}' must not be null, empty or whitespace.", nameof(storageName));

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