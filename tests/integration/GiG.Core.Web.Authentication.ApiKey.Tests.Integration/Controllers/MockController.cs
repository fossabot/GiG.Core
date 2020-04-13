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
        private readonly IActivityTenantAccessor _tenantAccessor;

        public MockController(IActivityTenantAccessor tenantAccessor)
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
