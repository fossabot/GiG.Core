using System;
using GiG.Core.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GiG.Core.Configuration.Tests.Integration.Tests
{
    public class HostBuilderExtensionsTests
    {
        private const string ConfigurationSectionName = "Configuration";

        public HostBuilderExtensionsTests()
        {
            Environment.SetEnvironmentVariable(ConfigurationSectionName, null);
        }
        
        [Fact] 
        public void OverrideConfigurationWithEnvironmentVariable()
        {
            // Arrange
            const string expected = "Environment";

            Environment.SetEnvironmentVariable(ConfigurationSectionName, expected);

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));           
        }

        [Fact]
        public void OverrideConfigurationWithAppSettingsOverride()
        {
            // Arrange
            const string expected = "Override";
                    
            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));
        }

        [Fact]
        public void UseDefaultConfigurationWithOutAppSettingsOverride()
        {
            // Arrange
            var expected = "AppSettings";

            var host = Host.CreateDefaultBuilder()               
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));
        }

        [Fact]
        public void GetDefaultConfigurationWhenPropertyIsNotInAppSettingsOverride()
        {
            // Arrange
            const string expected = "Debug";
            const string logLevelName = "LogLevel";

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(logLevelName));
        }
    }
}