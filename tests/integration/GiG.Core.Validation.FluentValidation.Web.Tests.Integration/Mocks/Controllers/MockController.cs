﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Validation.FluentValidation.Web.Tests.Integration.Mocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => throw new ValidationException("Test");
    }
}