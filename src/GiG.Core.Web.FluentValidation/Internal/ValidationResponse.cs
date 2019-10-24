using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.Benchmarks")]
namespace GiG.Core.Web.FluentValidation.Internal
{
    /// <summary>
    /// Validation Response.
    /// </summary>
    internal class ValidationResponse
    {
        /// <summary>
        /// The title.
        /// </summary>
        public string Title { get; set; } = "One or more validation errors occurred.";

        /// <summary>
        /// The Http Status.
        /// </summary>
        public int Status { get; set; } = (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// The Error Messages.
        /// </summary>
        public IDictionary<string, List<string>> Errors { get; set; }
    }
}