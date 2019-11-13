using GiG.Core.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace GiG.Core.Configuration.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HostBuilderExtensionsTests
    {
        private const string ConfigurationSectionName = "Configuration";

        public HostBuilderExtensionsTests()
        {
            Environment.SetEnvironmentVariable(ConfigurationSectionName, null);
        }

        [Fact]
        public void ConfigureExternalConfiguration_HostBuilderIsNull_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => HostBuilderExtensions.ConfigureExternalConfiguration(null));
        }

        [Fact] 
        public void GetValue_ConfigurationWithEnvironmentVariablesOverride_ReturnsOverride()
        {
            // Arrange
            const string expected = "Environment";

            Environment.SetEnvironmentVariable(ConfigurationSectionName, expected);

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();
            
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Act - Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));           
        }

        [Fact]
        public void GetValue_ConfigurationWithAppSettingsOverride_ReturnsOverride()
        {
            // Arrange
            const string expected = "Override";
                    
            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();
            
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Act - Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));
        }

        [Fact]
        public void GetValue_ConfigurationWithOutAppSettingsOverride_ReturnsDefault()
        {
            // Arrange
            const string expected = "AppSettings";

            var host = Host.CreateDefaultBuilder()               
                .Build();
            
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Act - Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));
        }

        [Fact]
        public void GetValue_WhenPropertyIsNotInAppSettingsOverride_ReturnsDefault()
        {
            // Arrange
            const string expected = "Debug";
            const string logLevelName = "LogLevel";

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Act - Assert
            Assert.Equal(expected, configuration.GetValue<string>(logLevelName));
        }
    }
}