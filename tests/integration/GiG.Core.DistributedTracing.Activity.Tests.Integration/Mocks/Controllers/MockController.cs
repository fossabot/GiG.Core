using GiG.Core.DistributedTracing.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        public MockController(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }

        [HttpGet]
        public ActionResult<ActivityResponse> GetActivity()
        {
            return new ActivityResponse
            {
                CorrelationId = _activityContextAccessor.CorrelationId,
                OperationName = _activityContextAccessor.OperationName,
                SpanId = _activityContextAccessor.SpanId,
                TraceId = _activityContextAccessor.TraceId
            };
        }
    }
}