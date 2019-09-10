using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GiG.Core.Web.Hosting.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        public MockController()
        {
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(HttpContext.Request.PathBase);
        }
        
        [HttpGet("ip")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public ActionResult<string> GetIp()
        {
            return Ok(HttpContext.Connection.RemoteIpAddress?.ToString());
        }
    }
}