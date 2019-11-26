﻿namespace GiG.Core.TokenManager.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenManagerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultSectionName = "TokenManager";
        
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public TokenClientOptions Client { get; set; } = new TokenClientOptions();
    }
}
