using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MockController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;

        public MockController(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(string.Join(",", _tenantAccessor.Values));
        }
    }
}
