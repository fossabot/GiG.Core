using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.Orleans.Streams")]

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// The Orleans Stream Helper.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// The Namespace Prefix.
        /// </summary>
        public static string NamespacePrefix { get; internal set; }

        /// <summary>
        /// Helper method to retrieve the namespace including prefix.
        /// </summary>
        /// <param name="namespace">The stream namespace.</param>
        public static string GetNamespace(string @namespace)
        {
            return NamespacePrefix == null ? @namespace : $"{NamespacePrefix}.{@namespace}";
        }

        /// <summary>
        /// Helper method to retrieve the namespace including prefix.
        /// </summary>
        /// <param name="domain">The domain of the stream.</param>
        /// <param name="streamType">The stream type.</param>
        /// <param name="version">The version number of the model.</param>
        public static string GetNamespace(string domain, string streamType, uint? version = 1)
        {
            var namespaceWithoutVersion = $"orleans.{domain}.{streamType}";

            return GetNamespace(version == null ? namespaceWithoutVersion : $"{namespaceWithoutVersion}.v{version}");
        }
    }
}