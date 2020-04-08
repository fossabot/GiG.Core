using System;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// Stream Provider.
    /// </summary>
    public interface IStreamIdProvider
    {
        /// <summary>
        /// Generates Stream Id using a Namespace and a key.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="key">The key.</param>
        /// <returns>The Stream Id.</returns>
        Guid NewStreamId(Guid @namespace, string key);

        /// <summary>
        /// Generates Stream Id.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="keys">A list of keys.</param>
        /// <returns>The Stream Id.</returns>
        Guid NewStreamId(Guid @namespace, string[] keys);
    }
}