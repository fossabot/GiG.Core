using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansCorrelationTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _clusterFixture;

        public OrleansCorrelationTests(ClusterFixture clusterFixture)
        {
            _clusterFixture = clusterFixture;
        }

        [Fact]
        public async Task GetCorrelationIdAsync_PublishGrainMessage_ReturnsExpectedCorrelationGuid()
        {        
            var grain = _clusterFixture.ClusterClient.GetGrain<IPublisherGrain>(123);

            var correlationId = Guid.NewGuid();

            RequestContext.ActivityId = correlationId;

            var result = await grain.PublishMessage(new Mocks.MockMessage());

            Assert.Equal(correlationId, result);
        }

        [Fact]
        public async Task GetCorrelationIdAsync_PublishGrainMessage_CorrelationIdShouldNotBeEmpty()
        {
            var grain = _clusterFixture.ClusterClient.GetGrain<IPublisherGrain>(123);
            
            var result = await grain.PublishMessage(new Mocks.MockMessage());

            Assert.NotEqual(Guid.Empty, result);
        }
    }
}