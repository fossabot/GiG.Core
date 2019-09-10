using GiG.Core.Hosting.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Hosting.Tests.Integration.Controllers
{
    [Route("api/management")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IApplicationMetadataAccessor _applicationMetadataAccessor;

        public ManagementController(IApplicationMetadataAccessor applicationMetadataAccessor)
        {
            _applicationMetadataAccessor = applicationMetadataAccessor;
        }

        [HttpGet("version")]
        public ActionResult<string> Version()
        {
            return Ok(_applicationMetadataAccessor.Version);
        }

        [HttpGet("name")]
        public ActionResult<string> Name()
        {
            return Ok(_applicationMetadataAccessor.Name);
        }

        [HttpGet("version-info")]
        public ActionResult<string> VersionInformational()
        {
            return Ok(_applicationMetadataAccessor.InformationalVersion);
        }
    }
}