using GiG.Core.DistributedTracing.Activity.Tests.Integration.Lifetimes;
using System;
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
    public class ActivityTests : IAsyncLifetime
    {
        private readonly ActivityLifetime _activityLifetime;
   
        public ActivityTests()
        {
            _activityLifetime = new ActivityLifetime();
        }

        public async Task InitializeAsync()
        {
            await _activityLifetime.InitializeAsync();
        }

        public async Task DisposeAsync()
        {
            await _activityLifetime.DisposeAsync();
        }

        [Fact]
        public void ActivityAccessor_ActivityNotSet_ShouldStillReturnNewActivity()
        {
            // Act
            var correlationId = _activityLifetime.ActivityContextAccessor.CorrelationId;

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
            var correlationId = _activityLifetime.ActivityContextAccessor.CorrelationId;

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
            var actualCorrelationId = _activityLifetime.ActivityContextAccessor.CorrelationId;
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
            var firstActualCorrelationId = _activityLifetime.ActivityContextAccessor.CorrelationId;
            var secondActualCorrelationId = _activityLifetime.ActivityContextAccessor.CorrelationId;
            
            activity.Stop();

            // Assert
            Assert.Equal(expectedCorrelationId, firstActualCorrelationId);
            Assert.Equal(expectedCorrelationId, secondActualCorrelationId);
        }

        [Fact]
        public async Task ActivityAccessor_HttpCall_ReturnsActivity()
        {
            // Arrange
            var testEndpointUrl = @$"{ActivityLifetime.BaseUrl}/api/mock";
            using var client = _activityLifetime.HttpClientFactory.CreateClient();

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

            var testEndpointUrl = @$"{ActivityLifetime.BaseUrl}/api/mock";
            using var client = _activityLifetime.HttpClientFactory.CreateClient();
            
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