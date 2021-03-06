﻿using GiG.Core.Logging.Extensions;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class LoggingConfigurationTests
    {
        [Fact]
        public void ConfigureLogging_ConfigurationSectionIsWrong_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                .ConfigureLogging(null, "Logging")
                    .Build());
            Assert.Equal("Configuration section 'Logging' is incorrect.", exception.Message);
        }
    }
}
