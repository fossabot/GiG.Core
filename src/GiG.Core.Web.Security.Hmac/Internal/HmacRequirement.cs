using Microsoft.AspNetCore.Authentication;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.Web.Security.Hmac.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GiG.Core.Web.Security.Hmac.Internal
{
    /// <summary>
    /// <see cref="AuthenticationOptions"/> for <see cref="HmacAuthenticationHandler"/>.
    /// </summary>
    internal class HmacRequirement: AuthenticationSchemeOptions
    {
    }
}
