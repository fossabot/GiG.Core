using Microsoft.Extensions.Logging;

namespace GiG.Core.Logging.Tests.Integration.Mocks
{
    public class MockClass
    {
        private readonly ILogger<MockClass> _logger;

        public MockClass(ILogger<MockClass> logger)
        {
            _logger = logger;
        }

        public void WriteLog()
        {
            _logger.LogInformation("This is a test");
        }
    }
}