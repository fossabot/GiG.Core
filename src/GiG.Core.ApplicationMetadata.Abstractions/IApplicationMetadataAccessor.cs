﻿namespace GiG.Core.ApplicationMetadata.Abstractions
{
    /// <summary>
    /// Accessor which provider version information about the application.
    /// </summary>
    public interface IApplicationMetadataAccessor
    {
        /// <summary>
        /// The application name as specified in the application's configuration.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// The application assembly version.
        /// </summary>
        string Version { get; set; }
        
        /// <summary>
        /// The application's informational version.
        /// </summary>
        string InformationalVersion { get; set; }
    }
}