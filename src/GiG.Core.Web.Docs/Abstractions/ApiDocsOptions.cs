namespace GiG.Core.Web.Docs.Abstractions
{
    /// <summary>
    /// Api Docs settings.
    /// </summary>
    public class ApiDocsOptions
    {
        /// <summary>
        /// The default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "ApiDocs";

        /// <summary>
        /// Indicates whether the ApiDocs are enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The ApiDocs Url.
        /// </summary>
        public string Url { get; set; } = "api-docs";

        /// <summary>
        /// The Docs Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Docs Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the IsForwardedFor filter is enabled or not.
        /// </summary>
        public bool IsForwardedForEnabled { get; set; } = true;
    }
}
