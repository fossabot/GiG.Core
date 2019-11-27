namespace GiG.Core.TokenManager
{
    internal static class Constants
    {
        public static class Errors
        {
            public const string InvalidGrant = "invalid_grant";
            public const string AccessTokenNullError = "Access Token is null for unknown reasons";
            public const string TokenEndpointNullError = "Token endpoint is null for unknown reasons";
        }
        
        public static class Logs
        {
            public const string AccessTokenRetrieving = "Creating access token";
            public const string AccessTokenRetrieved = "New Access Token retrieved";
            public const string RefreshTokenRetrieving = "Refreshing access token";
            public const string DiscoveryRetrieving = "Getting discovery information";
            public const string DiscoveryRetrieved = "Discovery retrieved";
        }
    }
}