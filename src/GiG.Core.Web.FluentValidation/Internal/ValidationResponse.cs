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
        /// Title.
        /// This is the default Fluent Validation title.
        /// </summary>
        public string Title { get; set; } = "One or more validation errors occurred.";

        /// <summary>
        /// Http Status.
        /// </summary>
        public int Status { get; set; } = (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// Error Messages.
        /// </summary>
        public IDictionary<string, List<string>> Errors { get; set; }
    }
}
