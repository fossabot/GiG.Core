using Microsoft.AspNetCore.Http;

namespace GiG.Core.Web.Security.Hmac.Abstractions
{
    /// <summary>
    /// HmacAuthentication option provider
    /// </summary>
    public interface IHmacOptionsProvider
    {
        /// <summary>
        /// Gets the current Hmac settings
        /// </summary>
        /// <returns><see cref="HmacOptions"/>.</returns>
        HmacOptions GetHmacOptions();
    }
}
