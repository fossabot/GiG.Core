using System.Linq;
using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.Sample.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/hell-world")]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = SecuritySchemes.ApiKey)]
    public class HelloWorldController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;
    
        public HelloWorldController(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }
        
        [HttpGet]
        public string Get()
        {
            return $"Hello Tenant {_tenantAccessor.Values.FirstOrDefault()}!";
        }
    }
}