using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Template.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Hello World !");
        }
    }
}