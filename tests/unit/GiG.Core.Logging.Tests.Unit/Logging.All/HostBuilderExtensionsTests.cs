using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit.Logging.All
{
    [Trait("Category", "Unit")]
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureLogging_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureLogging(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureLogging_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HostBuilder().ConfigureLogging(""));
            Assert.Equal("'sectionName' must not be null, empty or whitespace. (Parameter 'sectionName')", exception.Message);
        }
    }
}
