using Microsoft.AspNetCore.Authentication;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.Web.Authentication.Hmac.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GiG.Core.Web.Authentication.Hmac.Internal
{
    /// <summary>
    /// <see cref="AuthenticationOptions"/> for <see cref="HmacAuthenticationHandler"/>.
    /// </summary>
    internal class HmacRequirement: AuthenticationSchemeOptions
    {
    }
}