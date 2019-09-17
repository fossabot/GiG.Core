using GiG.Core.Context.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace GiG.Core.Request.Web.Internal
{
    internal class RequestContextAccessor : IRequestContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IPAddress IPAddress => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress;
    }
}
