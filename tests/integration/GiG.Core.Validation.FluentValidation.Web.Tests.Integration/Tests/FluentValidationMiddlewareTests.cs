using GiG.Core.Validation.FluentValidation.Web.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Validation.FluentValidation.Web.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class FluentValidationMiddlewareTests
    {
        private readonly TestServer _server;
        
        public FluentValidationMiddlewareTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task Validation_FluentValidationMiddlewareSetsResponseStatusToBadRequest_ReturnsBadRequestStatus()
        {
            // Arrange
            var client = _server.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            var body = response?.Content?.ReadAsStringAsync();

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(GetPropertyValue(body?.Result, "errorSummary"));
        }

        private static string GetPropertyValue(string json, string propertyName)
        {
            using var document = JsonDocument.Parse(json);
            var properties = document.RootElement.EnumerateObject();

            return properties.FirstOrDefault(x => x.Name.Equals(propertyName)).Value.ToString();
        }
    }
}