namespace GiG.Core.Web.Authentication.Hmac.Abstractions
{
    /// <summary>
    /// HmacAuthentication option provider.
    /// </summary>
    public interface IHmacOptionsProvider
    {
        /// <summary>
        /// Gets the current Hmac settings.
        /// </summary>
        /// <returns><see cref="HmacOptions"/>.</returns>
        HmacOptions GetHmacOptions();
    }
}
