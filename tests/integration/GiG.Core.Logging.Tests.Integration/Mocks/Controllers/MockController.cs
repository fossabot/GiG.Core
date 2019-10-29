using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Logging.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly ILogger _logger;

        public MockController(ILogger<MockController> logger) => _logger = logger;

        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.LogInformation("This is a test.");
            return Ok();
        }
    }
}