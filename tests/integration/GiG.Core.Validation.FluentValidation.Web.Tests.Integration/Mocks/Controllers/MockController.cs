using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Validation.FluentValidation.Web.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => throw new ValidationException(new[]
        {
            new ValidationFailure("test1", "error1.1"),
            new ValidationFailure("test2", "error2.1"),
            new ValidationFailure("test1", "error1.2")
        });
    }
}