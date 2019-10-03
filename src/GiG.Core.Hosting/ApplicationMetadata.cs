using System.Reflection;

namespace GiG.Core.Hosting
{
    /// <summary>
    /// Class which provides version information about the application.
    /// </summary>
    public static class ApplicationMetadata
    {
        private static string _name = Assembly.GetEntryAssembly()?.GetName().FullName;

        /// <summary>
        /// The application's name.
        /// </summary>
        public static string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// The application's assembly version.
        /// </summary>
        public static readonly string Version = Assembly.GetEntryAssembly()?.GetName().Version.ToString();

        /// <summary>
        /// The application's informational version.
        /// </summary>
        public static readonly string InformationalVersion = Assembly.GetEntryAssembly() ? .GetCustomAttribute<AssemblyInformationalVersionAttribute>() ? .InformationalVersion;
    }
}