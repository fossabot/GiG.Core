using GiG.Core.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureInfoManagement_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureInfoManagement(null, null));

            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureInfoManagement(null, configuration: null));
        }

        [Fact]
        public void ConfigureInfoManagement_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureInfoManagement(configuration: null));
        }

        [Fact]
        public void ConfigureInfoManagement_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureInfoManagement(configurationSection: null));
        }

        [Fact]
        public void AddApplicationMetadataAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddApplicationMetadataAccessor(null));
        }
    }
}
