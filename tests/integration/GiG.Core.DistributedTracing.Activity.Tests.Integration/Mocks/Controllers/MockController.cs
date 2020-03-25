using GiG.Core.DistributedTracing.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Constants = GiG.Core.MultiTenant.Abstractions.Constants;

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

        [HttpGet("tenants")]
        public ActionResult<IEnumerable<string>> GetTenantIds()
        {
            var tenantIds = 
                _activityContextAccessor.Baggage.Where(x => x.Key == Constants.TenantIdBaggageKey);

            return tenantIds.Select(x => x.Value).ToList();
        }
    }
}