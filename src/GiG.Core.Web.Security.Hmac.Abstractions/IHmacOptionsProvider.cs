using Microsoft.AspNetCore.Http;

namespace GiG.Core.Web.Security.Hmac.Abstractions
{
    public interface IHmacOptionsProvider
    {
        HmacOptions GetHmacOptions();
    }
}
