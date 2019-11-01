using System;

namespace GiG.Core.Security.Http
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
    }
}
