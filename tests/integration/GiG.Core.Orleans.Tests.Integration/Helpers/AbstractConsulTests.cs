using Bogus;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Text = System.Text;


namespace GiG.Core.Orleans.Tests.Integration.Helpers
{
    public abstract class AbstractConsulTests
    {
        protected string SiloName { get; set; }
        protected IClusterClient ClusterClient { get; set; }
        protected IHttpClientFactory HttpClientFactory { get; set; }
        protected string ConsulKvStoreBaseAddress { get; set; }

        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedInteger()
        {
            //Arrange
            var grain = ClusterClient.GetGrain<IEchoTestGrain>(Guid.NewGuid().ToString());
            var expectedValue = new Randomizer().Int();
            await grain.SetValueAsync(expectedValue);

            //Act 
            var actualValue = await grain.GetValueAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task GetSiloMembership_Consul_ReturnsSiloInformation()
        {
            //Arrange
            var expectedSiloName = SiloName;

            //Act
            using var client = HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ConsulKvStoreBaseAddress);

            var result = await client.GetStringAsync("orleans/dev?recurse=true");

            //Assert
            var results = JsonSerializer.Deserialize<IEnumerable<KVStoreResult>>(result);
            var silos = GetSilosFromResult(results);

            var actualSilo = silos.FirstOrDefault(s => s.SiloName == expectedSiloName);

            Assert.NotNull(result);
            Assert.NotNull(actualSilo);
            Assert.Equal(SiloStatus.Active, actualSilo.Status);
        }

        private static IEnumerable<Silo> GetSilosFromResult(IEnumerable<KVStoreResult> results)
        {
            return results
                .Where(x => x.Key.EndsWith("iamalive") == false && x.Key.EndsWith("version") == false)
                .Select(value => Convert.FromBase64String(value.Value))
                .Select(base64EncodedBytes => Text.Encoding.UTF8.GetString(base64EncodedBytes))
                .Select(decodedTxt => JsonSerializer.Deserialize<Silo>(decodedTxt))
                .ToList();
        }

        private class KVStoreResult
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        private class Silo
        {
            public string SiloName { get; set; }
            public SiloStatus Status { get; set; }
        }
    }
}
