using GiG.Core.DistributedTracing.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Tests.Unit.Tests
{
    [Trait("Category", "Integration")]
    public class TracingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureTracing_BuilderIsNull_ThrowsArgumentNullException()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureTracing(null, null, null));
            
            // Assert
            Assert.Equal("builder", exception.ParamName);
        }
        
        [Fact]
        public void ConfigureTracing_SectionNameMissing_ThrowsArgumentException()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder().ConfigureTracing(null, null));

            // Assert
            Assert.Equal("sectionName", exception.ParamName);
        }
    }
}