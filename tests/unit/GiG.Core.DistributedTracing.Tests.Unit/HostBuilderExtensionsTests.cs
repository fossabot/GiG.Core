using GiG.Core.DistributedTracing.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureTracing_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureTracing(null, null, null));
            Assert.Equal("builder", exception.ParamName);
        }
        
        [Fact]
        public void ConfigureTracing_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HostBuilder().ConfigureTracing(null, null));
            Assert.Equal("sectionName", exception.ParamName);
        }
    }
}
