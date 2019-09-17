using GiG.Core.Request.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace GiG.Core.Request.Web.Internal
{
    internal class RequestContextAccesor : IRequestContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContextAccesor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IPAddress IPAddress => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress;
    }
}
