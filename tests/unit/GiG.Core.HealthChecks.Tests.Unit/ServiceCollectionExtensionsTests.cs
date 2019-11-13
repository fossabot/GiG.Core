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
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureHealthChecks(null, null));
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureHealthChecks(null, configuration: null));
        }

        [Fact]
        public void ConfigureHealthChecks_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureHealthChecks(configuration: null));
        }

        [Fact]
        public void ConfigureHealthChecks_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureHealthChecks(configurationSection: null));
        }

        [Fact]
        public void AddCachedHealthChecks_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCachedHealthChecks(null));
        }
    }
}
