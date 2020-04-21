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
            var exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureLocalhostClustering(builder: (Orleans.Clustering.Abstractions.MembershipProviderBuilder<global::Orleans.IClientBuilder>)null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureLocalhostClustering(builder: (Orleans.Clustering.Abstractions.MembershipProviderBuilder<global::Orleans.Hosting.ISiloBuilder>)null, configuration: null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
