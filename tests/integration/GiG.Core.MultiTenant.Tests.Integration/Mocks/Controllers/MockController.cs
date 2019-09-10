using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.MultiTenant.Web.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;

        public MockController(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return _tenantAccessor.Values.ToList();
        }
    }
}