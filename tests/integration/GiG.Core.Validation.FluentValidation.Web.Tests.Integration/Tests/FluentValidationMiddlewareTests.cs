using GiG.Core.Web.FluentValidation.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Web.FluentValidation.Tests.Integration.Tests
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
            using var document = JsonDocument.Parse(body?.Result);
            var properties = document.RootElement.EnumerateObject();

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("One or more validation errors occurred.",
                GetPropertyValue(properties, "errorSummary").GetString());

            var errors = GetPropertyValue(properties, "errors");
            Assert.Equal("error1.1", errors.GetProperty("test1")[0].GetString());
            Assert.Equal("error1.2", errors.GetProperty("test1")[1].GetString());
            Assert.Equal("error2.1", errors.GetProperty("test2")[0].GetString());
        }

        private static JsonElement GetPropertyValue(JsonElement.ObjectEnumerator properties, string propertyName)
        {
            return properties.FirstOrDefault(x => x.Name.Equals(propertyName)).Value;
        }
    }
}