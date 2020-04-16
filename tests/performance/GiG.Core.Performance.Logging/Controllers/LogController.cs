using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Performance.Logging.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/log")]
    [ApiVersion("1")]
    public class LogController : ControllerBase
    {
        private const string LogText = "This is a test log!!!!!!!!!!!!!!!!!!!!11";

        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void EmitSingleLog()
        {
            _logger.LogInformation(LogText);
        }

        [HttpGet("{amount}")]
        public void EmitMultipleLogs([FromRoute] int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                _logger.LogInformation(LogText);
            }
        }
    }
}