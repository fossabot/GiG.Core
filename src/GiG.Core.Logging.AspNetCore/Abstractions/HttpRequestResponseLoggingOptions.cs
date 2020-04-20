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
        /// The Request Options.
        /// </summary>
        public HttpRequestResponseOptions Request { get; set; } = new HttpRequestResponseOptions();

        /// <summary>
        /// The Response Options.
        /// </summary>
        public HttpRequestResponseOptions Response { get; set; } = new HttpRequestResponseOptions();
    }
}
