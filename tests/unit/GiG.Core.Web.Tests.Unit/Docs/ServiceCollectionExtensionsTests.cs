using GiG.Core.Web.Docs.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.Docs
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureApiDocs_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureApiDocs(null, null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureApiDocs_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureApiDocs(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}
