﻿using System;

 namespace GiG.Core.TokenManager.Models
{
    /// <summary>
    /// Provides an OAuth2.0 JWT self-contained authentication token and
    /// its expiration time.
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// The current JWT self-contained token containing all the required
        /// information about the user, avoiding the need to perform further
        /// requests for additional information.
        /// </summary>
        public string AccessToken { get; internal set; }

        /// <summary>
        /// The refresh token used to get the next access token
        /// </summary>
        public string RefreshToken { get; internal set; }

        /// <summary>
        /// The expiration date and time after which the current token will not be valid.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; internal set; }
        
        /// <summary>
        /// The expiration in seconds when the current token will expiry.
        /// </summary>
        public int ExpiresIn { get; internal set; }
    }
}
