using GiG.Core.DistributedTracing.Abstractions.CorrelationId;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.DistributedTracing.Web.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public MockController(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return _correlationContextAccessor.CorrelationId.ToString();
        }
    }
}