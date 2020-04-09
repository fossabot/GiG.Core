﻿namespace GiG.Core.Web.Docs.Abstractions
{
    /// <summary>
    /// Api Docs Options.
    /// </summary>
    public class ApiDocsOptions
    {
        /// <summary>
        /// The default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "ApiDocs";

        /// <summary>
        /// A value to indicate if the ApiDocs are enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The ApiDocs Url.
        /// </summary>
        public string Url { get; set; } = "api-docs";

        /// <summary>
        /// The ApiDocs title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The ApiDocs description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A value to indicate if the IsForwardedFor is enabled or not.
        /// </summary>
        public bool IsForwardedForEnabled { get; set; } = true;
        
        /// <summary>
        /// A value to indicate if the XML inline documentation is enabled or not.
        /// </summary>
        public bool IsXmlDocumentationEnabled { get; set; } = true;

        /// <summary>
        /// A value to indicate if X-Tenant-ID is enabled or not.
        /// </summary>
        public bool XTenantIdEnabled { get; set; } = true;
        
        /// <summary>
        /// A value to indicate if X-Api-Key is enabled or not.
        /// </summary>
        public bool XApiKeyEnabled { get; set; } = false;
    }
}