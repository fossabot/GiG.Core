namespace GiG.Core.Web.Authentication.OAuth.Abstractions
{
    /// <summary>
    /// Api Authentication Options.
    /// </summary>
    public class OAuthAuthenticationOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Authentication:OAuth";

        /// <summary>
        /// The Authority.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// The Api Name.
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// The Api Secret.
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        /// The Scopes.
        /// </summary>
        public string Scopes { get; set; }

        /// <summary>
        /// The Supported Tokens.
        /// </summary>
        public string SupportedTokens { get; set; } = "Jwt";

        /// <summary>
        /// A value to indicate if the Api Authentication requires Https Metadata.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;

        /// <summary>
        /// The Legacy Audience Validation.
        /// </summary>
        public bool LegacyAudienceValidation { get; set; }
    }
}