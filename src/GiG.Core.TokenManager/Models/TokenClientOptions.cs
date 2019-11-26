namespace GiG.Core.TokenManager.Models
{
    public class TokenClientOptions
    {
        public string AuthorityUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scopes { get; set; } = "openid";

        public bool RequireHttps { get; set; } = true;

    }
}