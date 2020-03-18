using GiG.Core.Web.Authentication.ApiKey.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using Xunit;


namespace GiG.Core.Web.Tests.Unit.Authentication.ApiKey
{
    [Trait("Category", "Unit")]
    [Trait("Feature", "ApiKeyAuthentication")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddApiKeyAuthentication_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection nullServiceCollection = null;


            // Act / Assert
            Action unitUnderTest = () => ServiceCollectionExtensions.AddApiKeyAuthentication(nullServiceCollection);
            var exception = Assert.Throws<ArgumentNullException>(unitUnderTest);
            Assert.Equal("services", exception.ParamName);
        }


        [Fact]
        public void ConfigureDefaultApiKeyOptions_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ServiceCollection serviceCollection = null;
            IConfigurationSection configurationSection = null;

            // Act / Assert
            Action unitUnderTest = () => ServiceCollectionExtensions.ConfigureDefaultApiKeyOptions(serviceCollection, configurationSection);
            var exception = Assert.Throws<ArgumentNullException>(unitUnderTest);
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultApiKeyOptions_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IConfigurationSection configurationSection = null;
            ServiceCollection serviceCollection = new ServiceCollection();

            // Act / Assert
            Action unitUnderTest = () => serviceCollection.ConfigureDefaultApiKeyOptions(configurationSection);
            var exception = Assert.Throws<ConfigurationErrorsException>(unitUnderTest);
        }

        [Fact]
        public void ConfigureDefaultApiKeyOptions_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IConfiguration configuration = null;
            ServiceCollection serviceCollection = new ServiceCollection();

            // Act / Assert
            Action unitUnderTest = () => serviceCollection.ConfigureDefaultApiKeyOptions(configuration);
            var exception = Assert.Throws<ArgumentNullException>(unitUnderTest);
        }
    }
}
