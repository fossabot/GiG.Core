using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Sinks.File.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public sealed class LoggingFileSinkTests : IDisposable
    {
        private readonly string _logMessageTest = Guid.NewGuid().ToString();
        private readonly string _filePath;
        private readonly IHost _host;

        public LoggingFileSinkTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureLogging(x => x.WriteToFile())
                .Build();

            var configuration = _host.Services.GetRequiredService<IConfiguration>();
            _filePath = configuration["Logging:Sinks:File:FilePath"];

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            _host.Start();
        }

        [Fact]
        public async Task LogInformation_WriteLogToFile_VerifyContents()
        {
            // Arrange
            var logger = _host.Services.GetRequiredService<ILogger<LoggingFileSinkTests>>();
 
            // Act
            logger.LogInformation(_logMessageTest);
            Log.CloseAndFlush();

            //Assert
            Assert.True(File.Exists(_filePath));
            Assert.Contains(_logMessageTest, await File.ReadAllTextAsync(_filePath));
        }
        
        public void Dispose()
        {
            _host.StopAsync().Wait();
            _host.Dispose();
            File.Delete(_filePath);
        }
    }
}