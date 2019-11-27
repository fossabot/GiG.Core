namespace GiG.Core.TokenManager.Abstractions.Models
{
    /// <summary>
    /// Token Client Options.
    /// </summary>
    public class TokenClientOptions
    {
        /// <summary>
        /// The Authority URL.
        /// </summary>
        public string AuthorityUrl { get; set; }

        /// <summary>
        /// The Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The Client Secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The Scopes on which to grant access.
        /// </summary>
        public string Scopes { get; set; } = "openid";

        /// <summary>
        /// Require HTTPS.
        /// </summary>
        public bool RequireHttps { get; set; } = true;

    }
}