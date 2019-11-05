﻿using GiG.Core.Web.Security.Hmac.Abstractions;
using Microsoft.Extensions.Options;

namespace GiG.Core.Web.Security.Hmac.Internal
{
    /// <summary>
    /// Default Options provider for <see cref="HmacAuthenticationHandler"/>.
    /// </summary>
    internal class DefaultOptionsProvider : IHmacOptionsProvider
    {
        private readonly IOptionsSnapshot<HmacOptions> _optionsAccessor;

        /// <summary>
        /// Default Options provider for <see cref="HmacAuthenticationHandler"/>.
        /// </summary>
        public DefaultOptionsProvider(IOptionsSnapshot<HmacOptions> optionsAccessorAccessor)
        {
            _optionsAccessor = optionsAccessorAccessor;
        }

        /// <summary>
        /// Gets the configured hmac settings.
        /// </summary>
        /// <returns><see cref="HmacOptions"/></returns>
        public HmacOptions GetHmacOptions()
        {
            return _optionsAccessor.Value;
        }
    }
}