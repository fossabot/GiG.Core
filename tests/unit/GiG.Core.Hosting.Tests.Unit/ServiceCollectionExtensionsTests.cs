using GiG.Core.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
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
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureInfoManagement(null, null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureInfoManagement(null, configuration: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureInfoManagement_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureInfoManagement(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureInfoManagement_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new ServiceCollection().ConfigureInfoManagement(configurationSection: null));
            Assert.Equal("Configuration Section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void AddApplicationMetadataAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddApplicationMetadataAccessor(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
