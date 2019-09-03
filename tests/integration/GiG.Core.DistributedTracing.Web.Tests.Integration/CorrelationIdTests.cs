using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GiG.Core.DistributedTracing.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GiG.Core.DistributedTracing.Web.Tests.Integration
{
    public class CorrelationIdTests
    {
        private readonly TestServer _server;


        public CorrelationIdTests()
        {
            _server= new TestServer(new WebHostBuilder().UseStartup<MockStartup>());
        }

        [Fact]
        public async Task CorrelationIdGeneratedAndAddedToResponseHeader()
        {
            var client = _server.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock");
            using var response = await client.SendAsync(request);

            var headerValues = response.Headers.GetValues(Constants.Header);

            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);


            Guid guidResult;
            Assert.True(Guid.TryParse(headerValues.FirstOrDefault(), out guidResult));
        }

        [Fact]
        public async Task CorrelationIdInResponseHeaderMatchesRequestHeader()
        {
            var client = _server.CreateClient();

            var requestCorrelationId = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add(Constants.Header, requestCorrelationId);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/mock");
            using var response = await client.SendAsync(request);

            var headerValues = response.Headers.GetValues(Constants.Header);

            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);
            Assert.Equal(requestCorrelationId,headerValues.First());


        }
    }
}
