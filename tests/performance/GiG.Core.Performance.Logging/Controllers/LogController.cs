using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Performance.Logging.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/log")]
    [ApiVersion("1")]
     public class LogController : ControllerBase
     {
         private readonly ILogger<LogController> _logger;
         private const string LogText = "This is a test log";
         
         public LogController(ILogger<LogController> logger)
         {
             _logger = logger;
         }
         
         [HttpGet]
         public void Get()
         {
            _logger.LogInformation(LogText);
         }
         
         [HttpPost]
         public void Post()
         {
             _logger.LogInformation(LogText);
         }
     }
}