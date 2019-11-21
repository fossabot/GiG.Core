using GiG.Core.Logging.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class LoggingConfigurationTests
    {
        [Fact]
        public void ConfigureLogging_HostBuilderIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                .ConfigureLogging(null, "Logging")
                    .Build());
            Assert.Equal("Configuration section 'Logging' is not valid", exception.Message);
        }
    }
}
