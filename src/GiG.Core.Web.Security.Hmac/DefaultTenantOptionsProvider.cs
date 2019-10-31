using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Web.Security.Hmac.Abstractions
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
