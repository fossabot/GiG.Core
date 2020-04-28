using GiG.Core.Orleans.Reminders.Npgsql.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Reminders.Npgsql.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        private const string InvariantNamePostgreSQL = "Npgsql";

        /// <summary>
        /// Use Npgsql Reminder Service.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Reminder Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <returns>The<see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder UseNpgsqlReminderService([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            return builder.UseNpgsqlReminderService(storageName, configuration, NpgsqlOptions.DefaultSectionName);
        }

        /// <summary>
        /// Use Npgsql Reminder Service.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Reminder Storage name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <param name="sectionName">The Storage Providers section name.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder UseNpgsqlReminderService([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfiguration configuration, [NotNull] string sectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(storageName)) throw new ArgumentException($"'{nameof(storageName)}' must not be null, empty or whitespace.", nameof(storageName));
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException($"'{nameof(sectionName)}' must not be null, empty or whitespace.", nameof(sectionName));

            var namedStorageSectionName = $"{sectionName}:{storageName}";
            var storageConfigSection = configuration.GetSection(namedStorageSectionName);

            return builder.UseNpgsqlReminderService(storageName, storageConfigSection);
        }


        /// <summary>
        /// Use Npgsql Reminder Service.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Reminder Storage name.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="NpgsqlOptions"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder UseNpgsqlReminderService([NotNull] this ISiloBuilder builder, [NotNull] string storageName, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");
            if (string.IsNullOrWhiteSpace(storageName)) throw new ArgumentException($"'{nameof(storageName)}' must not be null, empty or whitespace.", nameof(storageName));

            var npgsqlOptions = configurationSection.Get<NpgsqlOptions>();

            if (npgsqlOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is not valid.");
            }

            builder.UseAdoNetReminderService(x =>
            {
                x.ConnectionString = npgsqlOptions.ConnectionString;
                x.Invariant = InvariantNamePostgreSQL;
            });

            return builder;
        }
    }
}