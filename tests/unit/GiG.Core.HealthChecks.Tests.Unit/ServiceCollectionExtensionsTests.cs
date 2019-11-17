using GiG.Core.HealthChecks.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureHealthChecks_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureHealthChecks(null, null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureHealthChecks(null, configuration: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureHealthChecks_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureHealthChecks(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureHealthChecks_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureHealthChecks(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void AddCachedHealthChecks_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCachedHealthChecks(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
