using Microsoft.AspNetCore.Authentication;

namespace GiG.Core.Web.Security.Hmac
{
    /// <summary>
    /// <see cref="AuthenticationOptions"/> for <see cref="HmacAuthenticationHandler"/>
    /// </summary>
    public class HmacRequirement: AuthenticationSchemeOptions
    {
    }
}
