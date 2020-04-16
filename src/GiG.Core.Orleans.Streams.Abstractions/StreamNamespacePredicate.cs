using Orleans.Streams;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// <see cref="IStreamNamespacePredicate"/> implementation allowing to filter stream namespaces using prefix.
    /// </summary>
    public class StreamNamespacePredicate : IStreamNamespacePredicate
    {
        private readonly string _streamNamespaceWithPrefix;

        /// <summary>
        /// Creates an instance of <see cref="StreamNamespacePredicate"/> with the specified namespace with prefix.
        /// </summary>
        /// <param name="streamNamespace">The stream namespace.</param>
        public StreamNamespacePredicate(string streamNamespace)
        {
            _streamNamespaceWithPrefix = StreamHelper.GetNamespace(streamNamespace);
        }
        
        /// <summary>
        /// Creates an instance of <see cref="StreamNamespacePredicate"/> with the specified namespace with prefix.
        /// </summary>
        /// <param name="domain">The domain of the stream.</param>
        /// <param name="streamType">The stream type.</param>
        /// <param name="version">The version number of the model.</param>
        public StreamNamespacePredicate(string domain, string streamType, uint version = 1)
        {
            _streamNamespaceWithPrefix = StreamHelper.GetNamespace(domain, streamType, version);
        }

        /// <inheritdoc />
        public bool IsMatch(string streamNamespace)
        {
            return _streamNamespaceWithPrefix == streamNamespace;
        }
    }
}