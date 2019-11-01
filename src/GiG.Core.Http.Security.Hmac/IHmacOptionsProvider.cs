namespace GiG.Core.Http.Security.Hmac
{
    /// <summary>
    /// <see cref="IHmacOptionsProvider"/> for <see cref="HmacDelegatingHandler"/>.
    /// </summary>
    public interface IHmacOptionsProvider
    {
        /// <summary>
        /// Gets the current HMAC settings.
        /// </summary>
        /// <returns><see cref="HmacOptions"/> for the current context.</returns>
        HmacOptions GetHmacOptions();
    }
}
