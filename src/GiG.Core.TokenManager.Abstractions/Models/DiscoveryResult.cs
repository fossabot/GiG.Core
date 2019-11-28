namespace GiG.Core.TokenManager.Abstractions.Models
{
    /// <summary>
    /// Provides the Discovery endpoint Result.
    /// </summary>
    public class DiscoveryResult
    {
        /// <summary>
        /// OAuth Token Endpoint Url from Discovery endpoint.
        /// </summary>
        public string TokenEndpoint { get; internal set; }
    }
}