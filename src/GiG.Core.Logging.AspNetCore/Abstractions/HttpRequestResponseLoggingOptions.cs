namespace GiG.Core.Logging.AspNetCore.Abstractions
{
    /// <summary>
    /// The Http Request Response Logging Options.
    /// </summary>
    public class HttpRequestResponseLoggingOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Logging:HttpRequestResponse";

        /// <summary>
        /// A value to indicate if Request Logging is enabled or not.
        /// </summary>
        public bool IsRequestLoggingEnabled { get; set; } = true;

        /// <summary>
        /// A value to indicate if the Request Logging includes Headers or not.
        /// </summary>
        public bool IncludeRequestHeaders { get; set; } = true;

        /// <summary>
        /// A value to indicate if the Request Logging includes the Request Body or not.
        /// </summary>
        public bool IncludeRequestBody { get; set; } = true;

        /// <summary>
        /// A value to indicate if Response Logging is enabled or not.
        /// </summary>
        public bool IsResponseLoggingEnabled { get; set; } = true;

        /// <summary>
        /// A value to indicate if the Reponse Logging includes Headers or not.
        /// </summary>
        public bool IncludeResponseHeaders { get; set; } = true;

        /// <summary>
        /// A value to indicate if the Reponse Logging includes the Response Body or not.
        /// </summary>
        public bool IncludeResponsetBody { get; set; } = true;
    }
}
