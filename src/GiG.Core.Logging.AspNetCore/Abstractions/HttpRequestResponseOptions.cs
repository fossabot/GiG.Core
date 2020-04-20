namespace GiG.Core.Logging.AspNetCore.Abstractions
{
    /// <summary>
    /// The Http Request Response Options.
    /// </summary>

    public class HttpRequestResponseOptions
    {
        /// <summary>
        /// A value to indicate if Logging is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// A value to indicate if the Logging includes Headers or not.
        /// </summary>
        public bool IncludeHeaders { get; set; } = true;

        /// <summary>
        /// A value to indicate if the Logging includes the Request Body or not.
        /// </summary>
        public bool IncludeBody { get; set; } = true;
    }
}