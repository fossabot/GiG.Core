using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.Security.Hmac.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MockController : ControllerBase
    {

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "";
        }

        [HttpPost]
        public ActionResult<string> Post(RequestModel model)
        {
            return model.Text;
        }
    }

    public class RequestModel
    {
        public string Text { get; set; }
    }
}