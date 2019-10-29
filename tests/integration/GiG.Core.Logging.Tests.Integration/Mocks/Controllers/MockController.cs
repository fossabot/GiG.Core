using GiG.Core.DistributedTracing.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Logging.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public MockController(ILogger<MockController> logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            _logger = logger;
            _correlationContextAccessor = correlationContextAccessor;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.LogInformation("This is a test.");
            return _correlationContextAccessor.Value.ToString();
        }
    }
}