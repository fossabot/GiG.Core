using Microsoft.Extensions.Options;

namespace GiG.Core.Http.Security.Hmac.Internal
{
    /// <inheritdoc />
    internal class DefaultHmacOptionsProvider : IHmacOptionsProvider
    {
        private readonly IOptionsSnapshot<HmacOptions> _optionsAccessor;

        /// <inheritdoc />
        public DefaultHmacOptionsProvider(IOptionsSnapshot<HmacOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }

        /// <inheritdoc />
        public HmacOptions GetHmacOptions() => _optionsAccessor.Value;
    }
}
