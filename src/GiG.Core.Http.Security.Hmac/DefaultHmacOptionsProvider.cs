using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Http.Security.Hmac
{
    public class DefaultHmacOptionsProvider : IHmacOptionsProvider
    {
        private readonly IOptionsSnapshot<HmacOptions> _options;

        public DefaultHmacOptionsProvider(IOptionsSnapshot<HmacOptions> options)
        {
            _options = options;
        }
        public HmacOptions GetHmacOptions() => _options.Value;
    }
}
