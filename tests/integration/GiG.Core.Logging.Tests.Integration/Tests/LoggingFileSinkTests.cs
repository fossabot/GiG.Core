using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Sinks.File.Extensions;
using GiG.Core.Logging.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class LoggingFileSinkTests
    {
        private readonly IHost _host;

        public LoggingFileSinkTests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                    services.AddSingleton<MockClass>()
                )
                .ConfigureLogging(x => x.WriteToFile())
                .Build();

            host.StartAsync().GetAwaiter().GetResult();

            _host = host;
        }

        [Fact]
        public async Task LoggingFileSinkTests_WriteLog()
        {
            // Arrange
            var path = $"logs\\logs.txt";

            // Act
            _host.Services.GetRequiredService<MockClass>().WriteLog();

            //Assert
            Assert.True(File.Exists(path));
            while (CanReadFile(path) is true)
            {
                Assert.Contains(await File.ReadAllTextAsync(path), "This is a test");
                File.Delete(path);
            }
        }

        // verify if file can be read
        private static bool CanReadFile(string path)
        {
            try
            {
                File.Open(path, FileMode.Open, FileAccess.Read).Dispose();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}