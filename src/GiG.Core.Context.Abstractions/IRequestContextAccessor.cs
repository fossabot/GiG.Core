using System.Net;

namespace GiG.Core.Context.Abstractions
{
    /// <summary>
    /// Request Context Accessor.
    /// </summary>
    public interface IRequestContextAccessor
    {
        /// <summary>
        /// Contains the IP Address of the originating request.
        /// </summary>
        IPAddress IPAddress { get; }
    }
}
