using Microsoft.Extensions.Options;

namespace GiG.Core.Http.Security.Hmac.Internal
{
    /// <inheritdoc />
    internal class DefaultHmacOptionsProvider : IHmacOptionsProvider
    {
        private readonly HmacOptions _optionsAccessor;

        /// <inheritdoc />
        public DefaultHmacOptionsProvider(HmacOptions optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }

        /// <inheritdoc />
        public HmacOptions GetHmacOptions() => _optionsAccessor;
    }
}
