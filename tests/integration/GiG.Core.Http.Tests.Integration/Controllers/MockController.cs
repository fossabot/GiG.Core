using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GiG.Core.Http.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] ITenantAccessor tenantAccessor)
        {
            Response.Headers.Add(Constants.Header, tenantAccessor.Values.ToArray());

            return NoContent();
        }
    }
}