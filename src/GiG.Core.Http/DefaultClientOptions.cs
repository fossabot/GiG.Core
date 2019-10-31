namespace GiG.Core.Http
{
    /// <summary>
    /// Default Client Options.
    /// </summary>
    public class DefaultClientOptions
    {
        /// <summary>
        /// Default Section Name
        /// </summary>
        public const string DefaultSectionName = "HttpClient";
        
        /// <summary>
        /// Base Url to initialise Http Client.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}