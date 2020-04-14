using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Benchmarks.Web
{
    [Route("validation")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        [HttpGet("small")]
        public ActionResult<string> GetSmall() => throw new ValidationException(new[]
        {
            new ValidationFailure("Address", "Address field is required.")
        });
        
        [HttpGet("large")]
        public ActionResult<string> GetLarge() => throw new ValidationException(new[]
        {
            new ValidationFailure("Name", "Name field should contain at least 2 characters."),
            new ValidationFailure("Name", "Name field cannot contain only numbers."),
            new ValidationFailure("Name", "Name field should contain at most 100 characters."),
            new ValidationFailure("Surname", "Surname field is required."),
            new ValidationFailure("Surname", "Surname field should contain at least 2 characters."),
            new ValidationFailure("Surname", "Surname field cannot contain only numbers."),
            new ValidationFailure("Surname", "Surname field should contain at most 100 characters."),
            new ValidationFailure("Address", "Address field is required."),
            new ValidationFailure("Address", "Address field should contain at least 2 characters."),
            new ValidationFailure("Address", "Address field should contain at most 500 characters."),
            new ValidationFailure("Age", "Age field should be greater than or equal 18."),
            new ValidationFailure("Age", "Age field should be less than or equal 100.")
        });
    }
}