using GiG.Core.Configuration.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Configuration.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureExternalConfiguration_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureExternalConfiguration(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
