using GiG.Core.DistributedTracing.Activity.Tests.Integration.Fixtures;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ActivityTests : IClassFixture<WebFixture>
    {
        private readonly WebFixture _fixture;
   
        public ActivityTests(WebFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void ActivityAccessor_ActivityNotSet_ShouldStillReturnNewActivity()
        {
            // Act
            var correlationId = _fixture.ActivityContextAccessor.CorrelationId;

            // Assert 
            Assert.NotEmpty(correlationId);
        }
        
        [Fact]
        public void ActivityAccessor_ActivityStopped_ShouldStillReturnNewActivity()
        {
            var activity = new System.Diagnostics.Activity("Tests");
            activity.Start();
            var initialCorrelationId = activity.RootId;
            activity.Stop();
            
            // Act
            var correlationId = _fixture.ActivityContextAccessor.CorrelationId;

            // Assert 
            Assert.NotEmpty(correlationId);
            Assert.NotEqual(initialCorrelationId, correlationId);
        }

        [Fact]
        public void ActivityAccessor_Activity_ReturnsCorrelationId()
        {
            // Arrange
            var activity = new System.Diagnostics.Activity("Tests");
            activity.Start();

            var expectedCorrelationId = activity.RootId;

            // Act
            var actualCorrelationId = _fixture.ActivityContextAccessor.CorrelationId;
            activity.Stop();

            // Assert
            Assert.Equal(expectedCorrelationId, actualCorrelationId);
        }
        
        
        [Fact]
        public void ActivityAccessor_ActivityCalledTwice_ReturnsSameCorrelationId()
        {
            // Arrange
            var activity = new System.Diagnostics.Activity("Tests");
            activity.Start();

            var expectedCorrelationId = activity.RootId;

            // Act
            var firstActualCorrelationId = _fixture.ActivityContextAccessor.CorrelationId;
            var secondActualCorrelationId = _fixture.ActivityContextAccessor.CorrelationId;
            
            activity.Stop();

            // Assert
            Assert.Equal(expectedCorrelationId, firstActualCorrelationId);
            Assert.Equal(expectedCorrelationId, secondActualCorrelationId);
        }

        [Fact]
        public async Task ActivityAccessor_HttpCall_ReturnsActivity()
        {
            // Arrange
            var testEndpointUrl = @$"{WebFixture.BaseUrl}/api/mock";
            using var client = _fixture.HttpClientFactory.CreateClient();

            //Act 
            var response = await client.GetAsync(testEndpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var actualCorrelationId = GetPropertyValue(responseBody, "correlationId");
            var actualTraceId = GetPropertyValue(responseBody, "traceId");
            var actualSpanId = GetPropertyValue(responseBody, "spanId");
            var actualOperationName = GetPropertyValue(responseBody, "operationName");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(actualCorrelationId));
            Assert.False(string.IsNullOrWhiteSpace(actualTraceId));
            Assert.False(string.IsNullOrWhiteSpace(actualSpanId));
            Assert.False(string.IsNullOrWhiteSpace(actualOperationName));
        }

        [Fact]
        public async Task Activity_StartedBeforeHttpCall_ReturnsMatchingActivity()
        {
            // Arrange
            var activity = new System.Diagnostics.Activity("Tests");
            activity.Start();
            var expectedCorrelationId = activity.RootId;

            var testEndpointUrl = @$"{WebFixture.BaseUrl}/api/mock";
            using var client = _fixture.HttpClientFactory.CreateClient();
            
            //Act 
            var response = await client.GetAsync(testEndpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            activity.Stop();

            var actualCorrelationId = GetPropertyValue(responseBody, "correlationId");
            var actualTraceId = GetPropertyValue(responseBody, "traceId");
            var actualSpanId = GetPropertyValue(responseBody, "spanId");
            var actualOperationName = GetPropertyValue(responseBody, "operationName");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedCorrelationId, actualCorrelationId);
            Assert.False(string.IsNullOrWhiteSpace(actualTraceId));
            Assert.False(string.IsNullOrWhiteSpace(actualSpanId));
            Assert.False(string.IsNullOrWhiteSpace(actualOperationName));
        }

        private static string GetPropertyValue(string json, string propertyName)
        {
            using var document = JsonDocument.Parse(json);
            var properties = document.RootElement.EnumerateObject();

            return properties.FirstOrDefault(x => x.Name.Equals(propertyName)).Value.ToString();
        }
    }
}