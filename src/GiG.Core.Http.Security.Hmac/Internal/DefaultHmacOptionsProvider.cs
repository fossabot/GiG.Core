using Microsoft.Extensions.Options;

namespace GiG.Core.Http.Security.Hmac.Internal
{
    /// <inheritdoc />
    internal class DefaultHmacOptionsProvider : IHmacOptionsProvider
    {
        private readonly IOptions<HmacOptions> _options;

        /// <inheritdoc />
        public DefaultHmacOptionsProvider(IOptions<HmacOptions> options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public HmacOptions GetHmacOptions() => _options.Value;
    }
}
