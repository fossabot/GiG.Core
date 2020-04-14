using System.Net;

namespace GiG.Core.Validation.FluentValidation.Web.Internal
{
    /// <summary>
    /// Constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The Status Code.
        /// </summary>
        public const int StatusCode = (int) HttpStatusCode.BadRequest;
        
        /// <summary>
        /// Problem And Json Mime Type.
        /// </summary>
        public const string ProblemJsonMimeType = "application/problem+json";
    }
}