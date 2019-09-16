namespace GiG.Core.Hosting.Abstractions
{
    /// <summary>
    /// Accessor which provider version information about the application.
    /// </summary>
    public interface IApplicationMetadataAccessor
    {
        /// <summary>
        /// The application's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The application's assembly version.
        /// </summary>
        string Version { get; }
        
        /// <summary>
        /// The application's informational version.
        /// </summary>
        string InformationalVersion { get; }
    }
}