using GiG.Core.Data.Migration.Abstractions;
using GiG.Core.Data.Migration.Evolve.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Data;

namespace GiG.Core.Data.Migration.Evolve.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="DatabaseMigration" /> to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="dbConnection">The <see cref="IDbConnection" /> to be used for the Database Migration.</param>
        /// <returns>The <see cref="MigrationOptionsBuilder" />.</returns>
        public static MigrationOptionsBuilder AddDbMigration([NotNull] this IServiceCollection services, [NotNull] IDbConnection dbConnection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            services.TryAddSingleton<IDatabaseMigration, DatabaseMigration>();
            var builder = MigrationOptionsBuilder.CreateMigrationBuilder(services, dbConnection);

            return builder;
        }
    }
}