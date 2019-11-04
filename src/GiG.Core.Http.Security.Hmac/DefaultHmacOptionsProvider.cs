using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Http.Security.Hmac
{
    /// <inheritdoc />
    public class DefaultHmacOptionsProvider : IHmacOptionsProvider
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
