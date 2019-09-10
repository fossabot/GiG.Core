using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace GiG.Core.MultiTenant.Web.Internal
{
    /// <inheritdoc />
    internal class TenantAccessor : ITenantAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <inheritdoc />
        public TenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public IImmutableSet<string> Values =>
            _httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(Constants.Header, out var value) != true
                ? ImmutableHashSet<string>.Empty
                : value.ToImmutableHashSet();
    }
}