namespace GiG.Core.Http.Security.Hmac
{
    /// <summary>
    /// <see cref="HmacOptions"/> for <see cref="HmacDelegatingHandler"/>
    /// </summary>
    public class HmacOptions
    {
        /// <summary>
        /// <see cref="HmacOptions"/> default section name.
        /// </summary>
        public const string DefaultSectionName = "Authentication:Hmac";
        /// <summary>
        /// Hash Algorithm for Hmac Authentication. Default is sha256.
        /// </summary>
        public string HashAlgorithm { get; set; } = "sha256";
        /// <summary>
        /// Secret used for Hmac Authentication.
        /// </summary>
        public string Secret { get; set; }
    }
}
