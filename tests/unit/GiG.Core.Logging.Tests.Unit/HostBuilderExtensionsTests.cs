using GiG.Core.Logging.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureLogging_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureLogging(null, null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => All.Extensions.HostBuilderExtensions.ConfigureLogging(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureLogging_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HostBuilder().ConfigureLogging(null, null));
            Assert.Equal("Missing sectionName.", exception.Message);
        }
    }
}
