using JetBrains.Annotations;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Adds the Activity Grain incoming filter.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static ISiloBuilder AddActivityIncomingFilter([NotNull] this ISiloBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.AddIncomingGrainCallFilter<ActivityIncomingGrainCallFilter>();
        }
    }
}