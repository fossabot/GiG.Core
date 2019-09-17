using GiG.Core.Hosting.Abstractions;

namespace GiG.Core.Hosting.Internal
{
    /// <inheritdoc />
    internal class ApplicationMetadataAccessor : IApplicationMetadataAccessor
    { 
        /// <inheritdoc />
        public string Name => ApplicationMetadata.Name;

        /// <inheritdoc />
        public string Version =>ApplicationMetadata.Version;

        /// <inheritdoc />
        public string InformationalVersion => ApplicationMetadata.InformationalVersion;
    }
}