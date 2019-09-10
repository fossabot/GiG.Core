using System.Collections.Generic;

namespace GiG.Core.Web.FluentValidation.Internal
{
    /// <summary>
    /// Validation Response.
    /// </summary>
    internal class ValidationResponse
    {
        /// <summary>
        /// Validation Error Messages.
        /// </summary>
        public IDictionary<string, List<string>> ValidationErrors { get; set; }
    }
}
