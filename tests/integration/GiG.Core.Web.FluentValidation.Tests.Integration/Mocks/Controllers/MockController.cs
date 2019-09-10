using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.FluentValidation.Tests.Integration.Mocks.Controllers
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
            throw new ValidationException("Test");
        }
    }
}