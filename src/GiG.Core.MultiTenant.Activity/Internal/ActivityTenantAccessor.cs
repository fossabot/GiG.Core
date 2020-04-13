using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using System.Collections.Immutable;
using System.Linq;
using Constants = GiG.Core.MultiTenant.Abstractions.Constants;

namespace GiG.Core.MultiTenant.Activity.Internal
{
    /// <summary>
    /// Tenant Accessor using the Activity Context.
    /// </summary>
    internal class ActivityTenantAccessor : IActivityTenantAccessor
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        public ActivityTenantAccessor(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }

        /// <inheritdoc />
        public IImmutableSet<string> Values =>
            _activityContextAccessor.Baggage
                                    .Where(x => x.Key == Constants.TenantIdBaggageKey)
                                    .Select(x => x.Value).ToImmutableHashSet();
    }
}