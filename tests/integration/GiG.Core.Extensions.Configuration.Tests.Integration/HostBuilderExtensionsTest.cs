using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GiG.Core.Extensions.Configuration.Tests.Integration
{
    public class HostBuilderExtensionsTest
    {
        private const string ConfigurationName = "Configuration";
        
        [Fact] 
        public void OverrideConfigurationWithEnvironmentVariable()
        {
            // Arrange
            var expected = "Environment";

            Environment.SetEnvironmentVariable(ConfigurationName, expected);

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationName));           
        }

        [Fact]
        public void OverrideConfigurationWithAppSettingsOverride()
        {
            // Arrange
            var expected = "Override";
                    
            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationName));
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
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationName));
        }

        [Fact]
        public void GetDefaultConfigurationWhenPropertyIsNotInAppSettingsOverride()
        {
            // Arrange
            var expected = "Debug";

            var logLevelName = "LogLevel";

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            // Assert
            Assert.Equal(expected, configuration.GetValue<string>(logLevelName));
        }
    }
}