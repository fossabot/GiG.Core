using GiG.Core.MultiTenant.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Constants = GiG.Core.MultiTenant.Abstractions.Constants;

namespace GiG.Core.Http.MultiTenant
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> that injects one or more X-Tenant-ID Headers into the request.
    /// </summary>
    public class TenantDelegatingHandler : DelegatingHandler
    {
        private readonly ITenantAccessor _activityTenantAccessor;

        /// <summary>
        /// A <see cref="DelegatingHandler"/> that injects one or more X-Tenant-ID Headers into the request.
        /// </summary>
        /// <param name="activityTenantAccessor">The <see cref="T:GiG.Core.MultiTenant.Abstractions.IActivityTenantAccessor" /> to use.</param>
        public TenantDelegatingHandler(ITenantAccessor activityTenantAccessor)
        {
            _activityTenantAccessor = activityTenantAccessor;   
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.Header))
            {
                request.Headers.Add(Constants.Header, _activityTenantAccessor.Values);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
