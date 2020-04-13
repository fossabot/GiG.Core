namespace GiG.Core.Authentication.Hmac.Abstractions
{
    /// <summary>
    /// The Constants for HMAC.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The Security Scheme.
        /// </summary>
        public const string SecurityScheme = "hmac";
        
        /// <summary>
        /// The Header.
        /// </summary>
        public const string Header = "Authorization";

        /// <summary>
        /// The Nonce.
        /// </summary>
        public const string Nonce = "Nonce";
    }
}