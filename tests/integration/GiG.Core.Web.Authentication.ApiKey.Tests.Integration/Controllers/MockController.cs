using GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MockController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get() => Ok();

        [HttpPost]
        public ActionResult<string> Post(RequestModel model) => Ok(model.Text);
    }
}
