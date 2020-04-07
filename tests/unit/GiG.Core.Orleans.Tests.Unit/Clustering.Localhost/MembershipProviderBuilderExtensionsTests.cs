using GiG.Core.Orleans.Clustering.Localhost.Extensions;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Localhost
{
    [Trait("Category", "Unit")]
    public class MembershipProviderBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureLocalhostClustering_MembershipProviderBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureLocalhostClustering(clientBuilder: null, configuration: null));
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureLocalhostClustering(siloBuilder: null, configuration: null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }
    }
}
