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
    public class OrleansCorrelationTests : CorrelationIdClusterLifetime
    {
        [Fact]
        public async Task GetCorrelationIdAsync_PublishGrainMessage_ReturnsExpectedCorrelationGuid()
        {        
            var grain = ClusterClient.GetGrain<IPublisherGrain>(123);

            var correlationId = Guid.NewGuid();

            RequestContext.ActivityId = correlationId;

            var result = await grain.PublishMessage(new MockMessage());

            Assert.Equal(correlationId, result);
        }

        [Fact]
        public async Task GetCorrelationIdAsync_PublishGrainMessage_CorrelationIdShouldNotBeEmpty()
        {
            var grain = ClusterClient.GetGrain<IPublisherGrain>(123);
            
            var result = await grain.PublishMessage(new MockMessage());

            Assert.NotEqual(Guid.Empty, result);
        }
    }
}