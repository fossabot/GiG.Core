using GiG.Core.Orleans.Storage.Npgsql.Configurations;
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
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage Instance Name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Grain Storage options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, string storageName, [NotNull] IConfiguration configuration)
        {
            return builder.AddNpgsqlGrainStorage(storageName, configuration, NpgsqlOptions.DefaultSectionName);
        }

        /// <summary>
        /// Adds a named Npgsql Grain Storage Provider.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="storageName">The Grain Storage Instance Name.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Grain Storage options.</param>
        /// <param name="sectionName">The Storage Providers section name.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddNpgsqlGrainStorage([NotNull] this ISiloBuilder builder, string storageName, [NotNull] IConfiguration configuration, string sectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var namedStoreageSectionName = $"{sectionName}:{storageName}";
            var npgsqlOptions = configuration.GetSection(namedStoreageSectionName).Get<NpgsqlOptions>();

            if (npgsqlOptions == null)
            {
                throw new ConfigurationErrorsException(
                    $"Configuration section '{namedStoreageSectionName}' is not valid");
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
