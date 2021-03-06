﻿using System.Reflection;

namespace GiG.Core.Hosting
{
    /// <summary>
    /// Provides version information about the application.
    /// </summary>
    public static class ApplicationMetadata
    {
        private static string _name = Assembly.GetEntryAssembly()?.GetName().Name;

        /// <summary>
        /// The application's name.
        /// </summary>
        public static string Name
        {
            get => _name;
            internal set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                }
            }
        }
        
        /// <summary>
        /// The application's checksum value. Which is generated upon build.
        /// </summary>
        public static string Checksum
        {
            get;
            internal set;
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