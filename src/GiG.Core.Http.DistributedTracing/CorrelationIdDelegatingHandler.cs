using GiG.Core.DistributedTracing.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.DistributedTracing
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationContextAccessor"></param>
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
