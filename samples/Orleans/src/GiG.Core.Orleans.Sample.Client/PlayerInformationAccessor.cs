using System;
using Microsoft.AspNetCore.Http;

namespace GiG.Core.Orleans.Sample.Client
{
    public class PlayerInformationAccessor : IPlayerInformationAccessor
    {
        private const string PlayerIdHeaderKey = "Player-ID";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayerInformationAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? PlayerId =>
            Guid.TryParse(_httpContextAccessor.HttpContext.Request.Headers[PlayerIdHeaderKey], out var result)
                ? result
                : default(Guid?);
    }
}