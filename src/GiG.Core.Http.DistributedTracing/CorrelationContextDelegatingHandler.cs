using GiG.Core.DistributedTracing.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.DistributedTracing
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> that injects an X-Correlation-ID Header into the request.
    /// </summary>
    public class CorrelationContextDelegatingHandler : DelegatingHandler
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        /// <summary>
        /// A <see cref="DelegatingHandler"/> that injects an X-Correlation-ID Header into the request.
        /// </summary>
        /// <param name="activityContextAccessor">The <see cref="T:GiG.Core.DistributedTracing.Abstractions.IActivityContextAccessor" /> to use.</param>
        public CorrelationContextDelegatingHandler(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.Header))
            {
                request.Headers.Add(Constants.Header, _activityContextAccessor.CorrelationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}