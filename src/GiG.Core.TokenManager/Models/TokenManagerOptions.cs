﻿namespace GiG.Core.TokenManager.Models
{
    public class TokenManagerOptions
    {
        public const string DefaultSectionName = "TokenManager";
        
        public string Username { get; set; }

        public string Password { get; set; }
        
        public TokenClientOptions Client { get; set; } = new TokenClientOptions();
    }
}
