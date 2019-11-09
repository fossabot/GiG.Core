using Microsoft.Extensions.Options;

namespace GiG.Core.Http.Security.Hmac.Internal
{
    /// <inheritdoc />
    internal class DefaultHmacOptionsProvider : IHmacOptionsProvider
    {
        private readonly HmacOptions _options;

        /// <inheritdoc />
        public DefaultHmacOptionsProvider(HmacOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public HmacOptions GetHmacOptions() => _options;
    }
}
