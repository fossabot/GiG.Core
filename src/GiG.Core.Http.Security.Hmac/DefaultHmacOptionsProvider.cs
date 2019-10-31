using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Http.Security.Hmac
{
    /// <inheritdoc />
    public class DefaultHmacOptionsProvider : IHmacOptionsProvider
    {
        private readonly IOptionsSnapshot<HmacOptions> _options;

        /// <inheritdoc />
        public DefaultHmacOptionsProvider(IOptionsSnapshot<HmacOptions> options)
        {
            _options = options;
        }
        /// <inheritdoc />
        public HmacOptions GetHmacOptions() => _options.Value;
    }
}
