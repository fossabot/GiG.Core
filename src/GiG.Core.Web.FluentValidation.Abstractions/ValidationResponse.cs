using System.Collections.Generic;

namespace GiG.Core.Web.FluentValidation.Abstractions
{
    /// <summary>
    /// Validation Responce
    /// </summary>
    public class ValidationResponse
    {
        /// <summary>
        /// Validation Error Messages
        /// </summary>
        public IDictionary<string, List<string>> ValidationErrors { get; set; }
    }
}
