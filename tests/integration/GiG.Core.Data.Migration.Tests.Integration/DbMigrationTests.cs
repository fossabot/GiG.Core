using GiG.Core.Data.Migration.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Xunit;

namespace GiG.Core.Data.Migration.Tests.Integration
{
    public class DbMigrationTests
    {
        [Fact]
        public void DBMigration_DefaultMigration()
        {
            using (var connection = new SqliteConnection("Data Source=testdefault.db;"))
            {
                var services = SetupServiceCollection();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection).AddDefaultMigrationOptions();

                dbMigrationBuilder.Migrate();

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
        public void DBMigration_WithCustomScriptLocation()
        {
            using (var connection = new SqliteConnection("Data Source=testcustomlocation.db;"))
            {
                var services = SetupServiceCollection();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection).AddLocation("CustomScripts");

                dbMigrationBuilder.Migrate();

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
        public void DBMigration_WithCustomMetaDataTableName()
        {
            using (var connection = new SqliteConnection("Data Source=testmetatablename.db;"))
            {
                var services = new ServiceCollection();
                services.AddLogging();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection)
                    .AddLocation("Scripts")
                    .AddMetadataTableName("testchanges");

                dbMigrationBuilder.Migrate();

                connection.Open();

                var scriptsExecuted = GetNumberOfScriptsExecuted(connection, "testchanges");
                var metaTableCreated = WasTableCreated(connection, "testchanges");

                connection.Close();

                Assert.Equal(1l, scriptsExecuted);
                Assert.True(metaTableCreated);
            }

            // Cleanup
            File.Delete("testmetatablename.db");
        }

        [Fact]
        public void DBMigration_Dsiabled()
        {
            using (var connection = new SqliteConnection("Data Source=testdisable.db;"))
            {
                var services = new ServiceCollection();
                services.AddLogging();
                var dbMigrationBuilder = services
                    .AddDbMigration(connection)
                    .DisableMigration();

                dbMigrationBuilder.Migrate();

                connection.Open();

                var metaTableCreated = WasTableCreated(connection, "changelog");

                connection.Close();

                Assert.False(metaTableCreated);
            }

            // Cleanup
            File.Delete("testdisable.db");
        }

        private IServiceCollection SetupServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment { EnvironmentName = "Development" });
            services.AddLogging();

            return services;
        }

        private long GetNumberOfScriptsExecuted(SqliteConnection connection, string metaTableName = "changelog")
        {
            SqliteCommand command = new SqliteCommand($"SELECT Count(id) FROM {metaTableName}", connection);
            var res = (long)command.ExecuteScalar();

            return res-1;
        }

        private bool WasTableCreated(SqliteConnection connection, string tableName)
        {
            SqliteCommand sqlCmd = new SqliteCommand($"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'", 
                connection);
            var tableCreated = sqlCmd.ExecuteScalar() != null;

            return tableCreated;
        }
    }
}
