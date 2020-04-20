﻿namespace GiG.Core.TokenManager.Abstractions.Models
{
    /// <summary>
    /// Token Manager Options.
    /// </summary>
    public class TokenManagerOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "TokenManager";
        
        /// <summary>
        /// The username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password.
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// The client options to use.
        /// </summary>
        public TokenClientOptions Client { get; set; } = new TokenClientOptions();
    }
}
