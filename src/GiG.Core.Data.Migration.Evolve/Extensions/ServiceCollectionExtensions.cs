using GiG.Core.Data.Migration.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Data;

namespace GiG.Core.Data.Migration.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="T:GiG.Core.Data.Migration.Evolce.DataBaseMigration" /> to the container.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="dbConnection">The <see cref="T:System.Data.IDbConnection" /> to be used for the DataBase migration.</param>
        /// <returns>The <see cref="T:GiG.Core.Data.Migration.Evolce.MigrationOptionsBuilder" /> to be used to build the Migration Options.</returns>
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
