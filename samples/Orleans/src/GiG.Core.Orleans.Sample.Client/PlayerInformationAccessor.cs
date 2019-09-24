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

        public string PlayerId => _httpContextAccessor.HttpContext.Request.Headers[PlayerIdHeaderKey];
    }
}