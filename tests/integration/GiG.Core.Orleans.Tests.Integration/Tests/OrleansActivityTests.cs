using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansActivityTests : IClassFixture<ActivityClusterFixture>
    {
        private readonly ActivityClusterFixture _fixture;

        public OrleansActivityTests(ActivityClusterFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task GetActivityAsync_NoParentActivity_ReturnsActivity()
        {
            // Arrange
            var grainId = Guid.NewGuid();
            var grain = _fixture.ClusterClient.GetGrain<IActivityTestGrain>(grainId);
            
            var activity = new Activity("test");
            activity.Start();

            // Act
            var activityResponse = await grain.GetActivityAsync();

            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotNull(activityResponse.TraceId);
            Assert.NotNull(activityResponse.RootId);
            Assert.False(string.IsNullOrEmpty(activityResponse.ParentId));
        }
        
        [Fact]
        public async Task GetActivityAsync_WithParentActivity_ReturnsActivityIncludingParentTraceId()
        {
            // Arrange
            var grainId = Guid.NewGuid();
            
            var activity = new Activity("test");
            activity.Start();

            var grain = _fixture.ClusterClient.GetGrain<IActivityTestGrain>(grainId);

            // Act
            var activityResponse = await grain.GetActivityAsync();

            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotNull(activityResponse.TraceId);
            Assert.Equal(activity.Id, activityResponse.ParentId);
            Assert.Equal(activity.RootId, activityResponse.RootId);
        }

        [Fact]
        public async Task GetStreamActivityAsync_PublishGrainMessage_ReturnsExpectedActivityInformation()
        {
            var grainId = Guid.NewGuid();
            
            var activity = new Activity("test");
            activity.Start();

            var streamProvider = _fixture.ClusterClient.GetStreamProvider(Constants.StreamProviderName);
            var streamFactory = (IStreamFactory) _fixture.ClientServiceProvider.GetService(typeof(IStreamFactory));
            var stream = streamFactory.GetStream<MockMessage>(streamProvider, grainId, Constants.ActivityStreamNamespace);

            await stream.PublishAsync(new MockMessage());
            var grain = _fixture.ClusterClient.GetGrain<IActivityTestGrain>(grainId);
            var activityResponse = await grain.GetStreamActivityAsync();
            
            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotNull(activityResponse.TraceId);
            Assert.NotNull(activityResponse.ParentId);
            Assert.Equal(activity.RootId, activityResponse.RootId);
        }

        [Fact]
        public async Task GetActivityAsync_WithParentActivityAndBaggage_ReturnsActivityIncludingParentTraceId()
        {
            // Arrange
            var grainId = Guid.NewGuid();
            var baggageValue = Guid.NewGuid().ToString();
            var tenantValue = Guid.NewGuid().ToString();
            
            var activity = new Activity("test");
            activity.AddBaggage("BaggageTest", baggageValue);
            activity.AddBaggage("TenantId", tenantValue);
            activity.Start();

            var grain = _fixture.ClusterClient.GetGrain<IActivityTestGrain>(grainId);

            // Act
            var activityResponse = await grain.GetActivityAsync();

            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotNull(activityResponse.TraceId);
            Assert.Equal(activity.Id, activityResponse.ParentId);
            Assert.Equal(activity.RootId, activityResponse.RootId);
            Assert.Equal(baggageValue, activityResponse.Baggage);
            Assert.Equal(tenantValue, activityResponse.TenantId);
        }

        [Fact]
        public async Task GetStreamActivityAsync_WithPublishGrainMessageAndBaggage_ReturnsExpectedActivityInformation()
        {
            var grainId = Guid.NewGuid();
            var baggageValue = Guid.NewGuid().ToString();
            var tenantValue = Guid.NewGuid().ToString();
            
            var activity = new Activity("test");
            activity.AddBaggage("BaggageTest", baggageValue);
            activity.AddBaggage("TenantId", tenantValue);
            activity.Start();

            var streamProvider = _fixture.ClusterClient.GetStreamProvider(Constants.StreamProviderName);
            var streamFactory = (IStreamFactory) _fixture.ClientServiceProvider.GetService(typeof(IStreamFactory));
            var stream = streamFactory.GetStream<MockMessage>(streamProvider, grainId, Constants.ActivityStreamNamespace);

            await stream.PublishAsync(new MockMessage());
            var grain = _fixture.ClusterClient.GetGrain<IActivityTestGrain>(grainId);
            var activityResponse = await grain.GetStreamActivityAsync();
            
            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotNull(activityResponse.TraceId);
            Assert.NotNull(activityResponse.ParentId);
            Assert.Equal(activity.RootId, activityResponse.RootId);
            Assert.Equal(baggageValue, activityResponse.Baggage);
            Assert.Equal(tenantValue, activityResponse.TenantId);
        }
    }
}