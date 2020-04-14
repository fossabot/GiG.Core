using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Constants = GiG.Core.Authentication.ApiKey.Abstractions.Constants;

namespace GiG.Core.Web.Sample.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/hello-world")]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = Constants.SecurityScheme)]
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