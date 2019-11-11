using GiG.Core.HealthChecks.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseHealthChecks_ApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UseHealthChecks(null));
        }
    }
}
