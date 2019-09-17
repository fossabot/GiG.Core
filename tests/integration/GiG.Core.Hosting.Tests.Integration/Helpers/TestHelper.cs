using System.Reflection;

namespace GiG.Core.Hosting.Tests.Integration.Helpers
{
    public static class TestHelper
    {
        public const string ApplicationName = "GiG.Core.Hosting.Tests.Integration.Custom";

        public static readonly string Version = Assembly.GetEntryAssembly()?.GetName().Version.ToString();

        public static readonly string InformationalVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}