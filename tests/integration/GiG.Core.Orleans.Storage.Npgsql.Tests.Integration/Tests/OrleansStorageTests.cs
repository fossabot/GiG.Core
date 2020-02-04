using Bogus;
using GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Contracts;
using GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Fixtures;
using Npgsql;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansStorageTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _clusterFixture;
        private const string StateQuery = "SELECT payloadjson FROM storage WHERE grainidextensionstring = @grainidextensionstring";

        public OrleansStorageTests(ClusterFixture clusterFixture)
        {
            _clusterFixture = clusterFixture;
        }

        [Fact]
        public async Task PostgresStorage_StateWritten()
        {
            //Arrange
            var grainId = Guid.NewGuid().ToString();
            var grain = _clusterFixture.ClusterClient.GetGrain<IStorageTestGrain>(grainId);
            var expectedValue = new Randomizer().String2(10);

            //Act 
            var returnedValue = await grain.StoreAndReturnValue(expectedValue);
            var dBStateValue = GetStateValueFromDB(grainId, _clusterFixture.ConnectionString);

            //Assert
            Assert.Equal(expectedValue, returnedValue);
            Assert.Equal(expectedValue, dBStateValue);
        }

        [Fact]
        public async Task PostgresStorage_StateNotWritten()
        {
            //Arrange
            var grainId = Guid.NewGuid().ToString();
            var grain = _clusterFixture.ClusterClient.GetGrain<IStorageTestGrain>(grainId);
            var expectedValue = new Randomizer().String2(10);

            //Act 
            var returnedValue = await grain.SetAndReturnValue(expectedValue);
            var dBStateValue = GetStateValueFromDB(grainId, _clusterFixture.ConnectionString);

            //Assert
            Assert.Equal(expectedValue, returnedValue);
            Assert.Equal(string.Empty, dBStateValue);
        }

        [Fact]
        public async Task PostgresStorage_CustomSectionName_StateWritten()
        {
            //Arrange
            var grainId = Guid.NewGuid().ToString();
            var grain = _clusterFixture.ClusterClient.GetGrain<IStorageCustomSectionTestGrain>(grainId);
            var expectedValue = new Randomizer().String2(10);

            //Act 
            var returnedValue = await grain.StoreAndReturnValue(expectedValue);
            var dBStateValue = GetStateValueFromDB(grainId, _clusterFixture.CustomSectionConnectionString);

            //Assert
            Assert.Equal(expectedValue, returnedValue);
            Assert.Equal(expectedValue, dBStateValue);
        }
     
        private string GetStateValueFromDB(string grainId, string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            
            connection.Open();

            var stateQueryCommand = connection.CreateCommand();
            stateQueryCommand.CommandText = StateQuery;
            // ReSharper disable once StringLiteralTypo
            stateQueryCommand.Parameters.Add(new NpgsqlParameter("grainidextensionstring", grainId));
            var result = stateQueryCommand.ExecuteScalar();

            connection.Close();

            if (result == null)
            {
                return string.Empty;
            }

            using var jsonStateDoc = JsonDocument.Parse(result.ToString());
            var stateElement = jsonStateDoc.RootElement.EnumerateObject().FirstOrDefault(x => x.Name.Equals("value",StringComparison.CurrentCultureIgnoreCase));

            return stateElement.Value.GetString();
        }
    }
}