using GiG.Core.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
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
        public void GetValue_ConfigurationWithAppSettingsOverrideNoFiles_ReturnsOverride()
        {
            // Arrange
            const string expected = "AppSettings";
            
            // Arrange
            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();
            
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            
            // Act - Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));
            
        }

        [Fact]
        public void GetValue_ConfigurationWithAppSettingsOverrideOneFile_ReturnsOverride()
        {
            // Arrange
            const string expected = "Override";
            
            File.WriteAllText($"{Directory.GetCurrentDirectory()}/configs/appsettings.override.json", @"{ ""Configuration"": ""Override"" }");
            
            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();
            
            File.Delete($"{Directory.GetCurrentDirectory()}/configs/appsettings.override.json");
            
            var configuration = host.Services.GetRequiredService<IConfiguration>();
           
            // Act - Assert
            Assert.Equal(expected, configuration.GetValue<string>(ConfigurationSectionName));
            
        }
        
        [Fact]
        public void GetValue_ConfigurationWithAppSettingsOverrideTwoFiles_ReturnsOverride()
        {
            // Arrange
            const string expected = "Override2";

            File.WriteAllText($"{Directory.GetCurrentDirectory()}/configs/appsettings.override.json", @"{ ""Configuration"": ""Override"" }");
            File.WriteAllText($"{Directory.GetCurrentDirectory()}/configs/appsettings.override2.json", @"{ ""Configuration"": ""Override2"" }");

            var host = Host.CreateDefaultBuilder()
                .ConfigureExternalConfiguration()
                .Build();
            
            File.Delete($"{Directory.GetCurrentDirectory()}/configs/appsettings.override.json");
            File.Delete($"{Directory.GetCurrentDirectory()}/configs/appsettings.override2.json");
            
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