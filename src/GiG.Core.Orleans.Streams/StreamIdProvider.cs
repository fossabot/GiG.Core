using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.System;
using System;

namespace GiG.Core.Orleans.Streams
{
    /// <inheritDoc />
    public class StreamIdProvider : IStreamIdProvider
    {
        /// <inheritDoc />
        public Guid NewStreamId(Guid @namespace, string key)
        {
            return GuidUtility.Create(@namespace, key);
        }

        /// <inheritDoc />
        public Guid NewStreamId(Guid @namespace, params string[] keys)
        {
            return GuidUtility.Create(@namespace, string.Join(string.Empty, keys));
        }
    }
}