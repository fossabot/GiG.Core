using System;
using Xunit;
using Console = GiG.Core.Logging.Sinks.Console.Extensions;
using File = GiG.Core.Logging.Sinks.File.Extensions;
using Fluentd = GiG.Core.Logging.Sinks.Fluentd.Extensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit.Sinks
{
    [Trait("Category", "Unit")]
    public class LoggingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void WriteToConsole_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Console.LoggingConfigurationBuilderExtensions.WriteToConsole(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void WriteToFile_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => File.LoggingConfigurationBuilderExtensions.WriteToFile(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void WriteToFluentd_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Fluentd.LoggingConfigurationBuilderExtensions.WriteToFluentd(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
