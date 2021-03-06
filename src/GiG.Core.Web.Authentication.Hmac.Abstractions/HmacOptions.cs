﻿namespace GiG.Core.Web.Authentication.Hmac.Abstractions
{
    /// <summary>
    /// Options for HmacAuthentication.
    /// </summary>
    public class HmacOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Authentication:Hmac";

        /// <summary>
        /// Secret used for Hmac Authentication.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Hash Algorithm. Default is sha256.
        /// </summary>
        public string HashAlgorithm { get; set; } = "sha256";
    }
}
