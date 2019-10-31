using GiG.Core.Web.Security.Hmac.Abstractions;
using Microsoft.Extensions.Options;

namespace GiG.Core.Web.Security.Hmac
{
    public class DefaultTenantOptionsProvider : IHmacOptionsProvider
    {
        private readonly IOptionsSnapshot<HmacOptions> _options;

        public DefaultTenantOptionsProvider(IOptionsSnapshot<HmacOptions> options)
        {
            _options = options;
        }
        public HmacOptions GetHmacOptions()
        {
            return _options.Value;
        }
    }
}
