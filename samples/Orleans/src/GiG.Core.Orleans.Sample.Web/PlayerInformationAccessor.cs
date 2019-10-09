using Microsoft.AspNetCore.Http;
using System;

namespace GiG.Core.Orleans.Sample.Web
{
    internal class PlayerInformationAccessor : IPlayerInformationAccessor
    {
        private const string PlayerIdHeaderKey = "Player-ID";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayerInformationAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid PlayerId =>
            Guid.TryParse(_httpContextAccessor.HttpContext.Request.Headers[PlayerIdHeaderKey], out var result)
                ? result
                : Guid.Empty;
    }
}