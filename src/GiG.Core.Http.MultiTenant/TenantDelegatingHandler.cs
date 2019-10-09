using GiG.Core.MultiTenant.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.MultiTenant
{
    /// <summary>
    /// 
    /// </summary>
    public class TenantDelegatingHandler : DelegatingHandler
    {
        private readonly ITenantAccessor _tenantAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantAccessor"></param>
        public TenantDelegatingHandler(ITenantAccessor tenantAccessor) => _tenantAccessor = tenantAccessor;

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.Header))
            {
                request.Headers.Add(Constants.Header, _tenantAccessor.Values);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
