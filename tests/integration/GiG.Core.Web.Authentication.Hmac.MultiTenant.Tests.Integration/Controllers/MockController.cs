using GiG.Core.Web.Authentication.Hmac.MultiTenant.Tests.Integration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.Authentication.Hmac.MultiTenant.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MockController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()
        {
            return NoContent();
        }

        [HttpPost]
        public ActionResult<string> Post(RequestModel model)
        {
            return Ok(model.Text);
        }
    }    
}