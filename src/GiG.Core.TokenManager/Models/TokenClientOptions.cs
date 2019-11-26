namespace GiG.Core.TokenManager.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenClientOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string AuthorityUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scopes { get; set; } = "openid";

        /// <summary>
        /// 
        /// </summary>
        public bool RequireHttps { get; set; } = true;

    }
}