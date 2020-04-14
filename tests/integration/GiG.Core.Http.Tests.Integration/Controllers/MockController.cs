using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Constants = GiG.Core.MultiTenant.Abstractions.Constants;

namespace GiG.Core.Http.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] IActivityContextAccessor activityContextAccessor, [FromServices] ITenantAccessor activityTenantAccessor)
        {
            Response.Headers.Add(Constants.Header, activityTenantAccessor.Values.ToArray());
            Response.Headers.Add(Core.DistributedTracing.Abstractions.Constants.Header, activityContextAccessor.CorrelationId);
 
            return NoContent();
        }
    }
}