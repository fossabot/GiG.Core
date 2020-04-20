namespace GiG.Core.Http
{
    /// <summary>
    /// Default Http Client Options.
    /// </summary>
    public class DefaultClientOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "HttpClient";
        
        /// <summary>
        /// Base Url to initialise Http Client.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}