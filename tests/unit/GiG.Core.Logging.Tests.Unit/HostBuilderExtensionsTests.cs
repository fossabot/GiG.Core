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
            Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureLogging(null, null));
            Assert.Throws<ArgumentNullException>(() => All.Extensions.HostBuilderExtensions.ConfigureLogging(null, null));
        }

        [Fact]
        public void ConfigureLogging_SectionNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HostBuilder().ConfigureLogging(null, null));
        }
    }
}
