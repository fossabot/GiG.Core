using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using System;
using Xunit;
using ApplicationBuilderExtensions = GiG.Core.Web.Hosting.Extensions.ApplicationBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.Hosting
{
    [Trait("Category", "Unit")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UsePathBaseFromConfiguration_ApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UsePathBaseFromConfiguration(null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UsePathBaseFromConfiguration(null, ""));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void UsePathBaseFromConfiguration_ConfigSectionNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ApplicationBuilder(null).UsePathBaseFromConfiguration(null));
            Assert.Equal("'configSectionName' must not be null, empty or whitespace. (Parameter 'configSectionName')", exception.Message);
        }
    }
}