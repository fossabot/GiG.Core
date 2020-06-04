using GiG.Core.TokenManager.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.TokenManager.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddTokenManager_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddTokenManager(null, null));
            Assert.Equal("services", exception.ParamName);
        }
        
        [Fact]
        public void AddTokenManager_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new ServiceCollection().AddTokenManager(null));
            Assert.Equal("Configuration Section '' is incorrect.", exception.Message);
        }
        
        [Fact]
        public void AddTokenManager_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddTokenManager(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void AddTokenManagerFactory_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddTokenManagerFactory(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}