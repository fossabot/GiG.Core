using GiG.Core.Data.Migration.Evolve.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Xunit;
// ReSharper disable StringLiteralTypo

namespace GiG.Core.Data.Migration.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class DbMigrationTests
    {
        [Fact]
        public void Migrate_DefaultMigration()
        {
            using (var connection = new SqliteConnection("Data Source=testdefault.db;"))
            {
                // Arrange
                var services = SetupServiceCollection();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection).AddDefaultMigrationOptions();

                // Act
                dbMigrationBuilder.Migrate();

                // Assert
                connection.Open();

                var scriptsExecuted = GetNumberOfScriptsExecuted(connection);
                var metaTableCreated = WasTableCreated(connection, "changelog");
                var scriptExecuted = WasTableCreated(connection, "player");
                var devScriptExecuted = WasTableCreated(connection, "devplayer");

                connection.Close();

                Assert.Equal(2L, scriptsExecuted);
                Assert.True(metaTableCreated);
                Assert.True(scriptExecuted);
                Assert.True(devScriptExecuted);
            }

            // Cleanup
            File.Delete("testdefault.db");
        }

        [Fact]
        public void Migrate_WithCustomScriptLocation()
        {
            using (var connection = new SqliteConnection("Data Source=testcustomlocation.db;"))
            {
                // Arrange
                var services = SetupServiceCollection();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection).AddLocation("CustomScripts");

                // Act
                dbMigrationBuilder.Migrate();

                // Assert
                connection.Open();

                var scriptsExecuted = GetNumberOfScriptsExecuted(connection);
                var metaTableCreated = WasTableCreated(connection, "changelog");
                var customScriptExecuted = WasTableCreated(connection, "customplayer");

                connection.Close();

                Assert.Equal(1L, scriptsExecuted);
                Assert.True(metaTableCreated);
                Assert.True(customScriptExecuted);
            }

            // Cleanup
            File.Delete("testcustomlocation.db");
        }

        [Fact]
        public void Migrate_WithCustomMetaDataTableName()
        {
            using (var connection = new SqliteConnection("Data Source=testmetatablename.db;"))
            {
                // Arrange
                var services = new ServiceCollection();
                services.AddLogging();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection)
                    .AddLocation("Scripts")
                    .AddMetadataTableName("testchanges");

                // Act
                dbMigrationBuilder.Migrate();

                // Assert
                connection.Open();

                var scriptsExecuted = GetNumberOfScriptsExecuted(connection, "testchanges");
                var metaTableCreated = WasTableCreated(connection, "testchanges");

                connection.Close();

                Assert.Equal(1L, scriptsExecuted);
                Assert.True(metaTableCreated);
            }

            // Cleanup
            File.Delete("testmetatablename.db");
        }

        [Fact]
        public void Migrate_IsDisabled()
        {
            using (var connection = new SqliteConnection("Data Source=testdisable.db;"))
            {
                // Arrange
                var services = new ServiceCollection();
                services.AddLogging();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection)
                    .DisableMigration();

                // Act
                dbMigrationBuilder.Migrate();

                // Assert
                connection.Open();

                var metaTableCreated = WasTableCreated(connection, "changelog");

                connection.Close();

                Assert.False(metaTableCreated);
            }

            // Cleanup
            File.Delete("testdisable.db");
        }

        private static IServiceCollection SetupServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHostEnvironment>(new HostingEnvironment { EnvironmentName = "Development" });
            services.AddLogging();

            return services;
        }

        private static long GetNumberOfScriptsExecuted(SqliteConnection connection, string metaTableName = "changelog")
        {
            using var command = new SqliteCommand($"SELECT Count(id) FROM {metaTableName}", connection);
            var res = (long)command.ExecuteScalar();

            return res-1;
        }

        private static bool WasTableCreated(SqliteConnection connection, string tableName)
        {
            using var sqlCmd = new SqliteCommand($"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'", connection);
            var tableCreated = sqlCmd.ExecuteScalar() != null;

            return tableCreated;
        }
    }
}
