using System.Collections.Generic;

namespace GiG.Core.Models
{
    /// <summary>
    /// Error Response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// The Error Summary.
        /// </summary>
        public string ErrorSummary { get; set; } = "One or more validation errors occurred.";

        /// <summary>
        /// The Error Messages.
        /// </summary>
        public IDictionary<string, List<string>> Errors { get; set; }
    }
}