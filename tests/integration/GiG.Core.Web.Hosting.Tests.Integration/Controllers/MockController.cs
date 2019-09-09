﻿using Microsoft.AspNetCore.Http;
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
        
        [HttpGet("ip")]
        public ActionResult<string> GetIp()
        {
            return Ok(HttpContext.Connection.RemoteIpAddress.ToString());
        }
    }
}