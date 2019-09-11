using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Data;

namespace GiG.Core.Data.Migration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static MigrationOptionsBuilder AddDbMigration([NotNull] this IServiceCollection services, [NotNull] IDbConnection dbConnection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

            services.TryAddSingleton<DatabaseMigration>();
            var builder = MigrationOptionsBuilder.CreateMigrationBuilder(services, dbConnection);

            return builder;
        }
    }
}
