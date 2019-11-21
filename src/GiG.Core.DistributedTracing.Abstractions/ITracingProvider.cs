using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.DistributedTracing.Abstractions
{
    /// <summary>
    /// Tracing Provider.
    /// </summary>
    public interface ITracingProvider
    {
        /// <summary>
        /// Register Tracing Provider.
        /// </summary>
        void RegisterProvider(string provider);
    }
}