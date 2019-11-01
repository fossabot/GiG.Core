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
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class LoggingFileSinkTests : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _filePath;
        private readonly IHost _host;

        public LoggingFileSinkTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                    services.AddLogging()
                )
                .ConfigureLogging(x => x.WriteToFile())
                .Build();

            var configuration = _host.Services.GetRequiredService<IConfiguration>();
            _filePath = configuration["Logging:Sinks:File:FilePath"];

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            _host.Start();

            _logger = _host.Services.GetRequiredService<ILogger<LoggingFileSinkTests>>();
        }

        [Fact]
        public async Task LogInformation_WriteLog_VerifyContents()
        {
            // Arrange
            var logString = Guid.NewGuid().ToString();
 
            // Act
            _logger.LogInformation(logString);
            Log.CloseAndFlush();

            //Assert
            Assert.True(File.Exists(_filePath));
            Assert.Contains(logString, await File.ReadAllTextAsync(_filePath));
        }
        
        public void Dispose()
        {
            _host.StopAsync().Wait();
            _host.Dispose();
            File.Delete(_filePath);
        }
    }
}