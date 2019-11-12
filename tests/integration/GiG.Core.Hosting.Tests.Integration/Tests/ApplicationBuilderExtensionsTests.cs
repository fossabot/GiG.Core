using GiG.Core.Hosting.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseInfoManagement_ApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                    () => ApplicationBuilderExtensions.UseInfoManagement(null));
        }
    }
}
