using GiG.Core.MultiTenant.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.MultiTenant
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> that injects one or more X-Tenant-ID Headers into the request.
    /// </summary>
    public class TenantDelegatingHandler : DelegatingHandler
    {
        private readonly ITenantAccessor _tenantAccessor;

        /// <summary>
        /// A <see cref="DelegatingHandler"/> that injects one or more X-Tenant-ID Headers into the request.
        /// </summary>
        /// <param name="tenantAccessor">The <see cref="T:GiG.Core.MultiTenant.Abstractions.ITenantAccessor" /> to use.</param>
        public TenantDelegatingHandler(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;   
        }

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
