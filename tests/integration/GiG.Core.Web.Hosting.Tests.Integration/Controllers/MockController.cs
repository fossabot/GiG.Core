using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}