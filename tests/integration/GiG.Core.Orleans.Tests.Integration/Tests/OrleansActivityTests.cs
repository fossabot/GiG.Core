using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansActivityTests : ActivityClusterLifetime
    {
        [Fact]
        public async Task GetActivityAsync_NoParentActivity_ReturnsActivity()
        {
            // Arrange
            var grain = ClusterClient.GetGrain<IActivityTestGrain>("activity_test");

            // Act
            var activityResponse = await grain.GetActivityAsync();

            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotEmpty(activityResponse.TraceId);
            Assert.Empty(activityResponse.ParentId);
        }

        [Fact]
        public async Task GetActivityAsync_WithParentActivity_ReturnsActivityIncludingParentTraceId()
        {
            // Arrange
            var activity = new System.Diagnostics.Activity("test");
            activity.Start();
            var grain = ClusterClient.GetGrain<IActivityTestGrain>("activity_test");

            // Act
            var activityResponse = await grain.GetActivityAsync();

            // Assert
            Assert.NotNull(activityResponse);
            Assert.NotEmpty(activityResponse.TraceId);
            Assert.Equal(activity.RootId.ToString(), activityResponse.ParentId);
        }
    }
}