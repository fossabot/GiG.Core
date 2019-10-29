using GiG.Core.Logging.Tests.Integration.Extensions;
using GiG.Core.Logging.Tests.Integration.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class EnricherTests : IClassFixture<LoggingFixture>
    {
        private readonly LoggingFixture _loggingFixture;

        public EnricherTests(LoggingFixture loggingFixture) => _loggingFixture = loggingFixture;

        [Fact]
        public void TestEnrichers()
        {
            _loggingFixture.Logger.LogError("This is a test.");
            var logEvent = _loggingFixture.LogEvent;

            Assert.NotNull(logEvent);
            Assert.NotEmpty((string)logEvent.Properties["ApplicationName"].LiteralValue());
            Assert.NotEmpty((string)logEvent.Properties["ApplicationVersion"].LiteralValue());
            Assert.NotEmpty((string)logEvent.Properties["CorrelationId"].LiteralValue());
            Assert.NotEmpty((string)logEvent.Properties["IPAddress"].LiteralValue());
        }
    }
}
