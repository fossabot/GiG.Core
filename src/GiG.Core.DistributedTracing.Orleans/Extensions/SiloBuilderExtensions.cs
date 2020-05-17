using JetBrains.Annotations;
using Orleans.Hosting;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Adds the Activity Grain incoming filter.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddActivityIncomingFilter([NotNull] this ISiloBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.AddIncomingGrainCallFilter<ActivityIncomingGrainCallFilter>();
        }
        
        /// <summary>
        /// Adds the Activity Grain outgoing filter.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddActivityOutgoingFilter([NotNull] this ISiloBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.AddOutgoingGrainCallFilter<ActivityOutgoingGrainCallFilter>();
        }
        
        /// <summary>
        /// Adds the Activity Grain incoming and outgoing filters.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder AddActivityFilters([NotNull] this ISiloBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddIncomingGrainCallFilter<ActivityIncomingGrainCallFilter>();
            builder.AddOutgoingGrainCallFilter<ActivityOutgoingGrainCallFilter>();

            return builder;
        }
    }
}