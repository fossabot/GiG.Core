namespace GiG.Core.Security.Hmac.Abstractions
{
    /// <summary>
    /// Signature provider for Hmac.
    /// </summary>
    public interface IHmacSignatureProvider
    {
        /// <summary>
        /// Generates the signature for Hmac Authentication.
        /// </summary>
        /// <param name="method">The Http Method (GET,POST,PUT,PATCH).</param>
        /// <param name="path">The path for the Http request.</param>
        /// <param name="body">The Http request body.</param>
        /// <param name="nonce">The nonce value.</param>
        /// <param name="secret">The Hmac shared secret.</param>
        /// <returns></returns>
        string GetSignature(string method, string path, string body, string nonce, string secret);
    }
}
