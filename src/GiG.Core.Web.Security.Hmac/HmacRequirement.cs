using Microsoft.AspNetCore.Authentication;

namespace GiG.Core.Web.Security.Hmac
{
    /// <summary>
    /// <see cref="AuthenticationOptions"/> for <see cref="HmacAuthenticationHandler"/>.
    /// </summary>
    internal class HmacRequirement: AuthenticationSchemeOptions
    {
    }
}
