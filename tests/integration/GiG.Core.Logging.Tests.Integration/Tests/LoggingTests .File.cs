using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    public partial class LoggingTests
    {
        [Fact]
        public async Task LogInformation_WriteLogToFile_VerifyContents()
        {
            // Arrange
            var logger = _host.Services.GetRequiredService<ILogger<LoggingTests>>();
 
            // Act
            logger.LogInformation(_logMessageTest);
            Log.CloseAndFlush();

            //Assert
            Assert.True(File.Exists(_filePath));
            Assert.Contains(_logMessageTest, await File.ReadAllTextAsync(_filePath));
        }
    }
}