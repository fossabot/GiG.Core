using GiG.Core.Hosting.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void UseApplicationMetadata_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => HostBuilderExtensions.UseApplicationMetadata(null));
        }
    }
}
