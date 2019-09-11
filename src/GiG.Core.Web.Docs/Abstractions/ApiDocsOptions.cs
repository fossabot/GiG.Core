namespace GiG.Core.Web.Docs.Abstractions
{
    /// <summary>
    /// Api Docs settings.
    /// </summary>
    public class ApiDocsOptions
    {
        public const string DefaultSectionName = "ApiDocs";

        public bool IsEnabled { get; set; } = true;

        public string DocUrl { get; set; } = "api-docs";

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsForwardedForEnabled { get; set; } = true;
    }
}
