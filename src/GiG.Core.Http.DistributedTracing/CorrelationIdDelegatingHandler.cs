using GiG.Core.DistributedTracing.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.DistributedTracing
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> that injects an X-Correlation-ID Header into the request.
    /// </summary>
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// A <see cref="DelegatingHandler"/> that injects an X-Correlation-ID Header into the request.
        /// </summary>
        /// <param name="correlationContextAccessor">The <see cref="ICorrelationContextAccessor" />.</param>
        public CorrelationIdDelegatingHandler(ICorrelationContextAccessor correlationContextAccessor) => _correlationContextAccessor = correlationContextAccessor;

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.Header))
            {
                request.Headers.Add(Constants.Header, _correlationContextAccessor.Value.ToString());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}