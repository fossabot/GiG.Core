using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Web.Sample.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/hell-world")]
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = SecuritySchemes.ApiKey)]
    public class HelloWorldController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantAccessor"></param>
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