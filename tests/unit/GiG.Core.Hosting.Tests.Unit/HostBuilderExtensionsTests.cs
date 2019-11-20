using GiG.Core.Hosting.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void UseApplicationMetadata_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.UseApplicationMetadata(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
