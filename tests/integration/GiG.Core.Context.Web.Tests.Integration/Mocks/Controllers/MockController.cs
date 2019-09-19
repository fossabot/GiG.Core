using GiG.Core.Context.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Context.Web.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        private readonly IRequestContextAccessor _requestContextAccessor;

        public MockController(IRequestContextAccessor requestContextAccessor)
        {
            _requestContextAccessor = requestContextAccessor;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return _requestContextAccessor.IPAddress?.ToString();
        }
    }
}