namespace GiG.Core.Security.Hmac.Abstractions
{
    /// <summary>
    /// The Hmac Constants.
    /// </summary>
    public class HmacConstants
    {
        /// <summary>
        /// The Header for HmacAuthentication.
        /// </summary>
        public const string AuthHeader = "Authorization";

        /// <summary>
        /// The Nonce header for HmacAuthentication.
        /// </summary>
        public const string NonceHeader = "Nonce";

        /// <summary>
        /// The HMAC scheme.
        /// </summary>
        public const string Scheme = "hmac";
    }
}
