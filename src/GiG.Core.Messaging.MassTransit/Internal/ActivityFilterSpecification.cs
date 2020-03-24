using GreenPipes;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    internal class ActivityFilterSpecification<T> : IPipeSpecification<T>
        where T : class, PipeContext
    {
        private readonly TracerFactory _tracerFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tracerFactory">The <see cref="TracerFactory"/> used to get the <see cref="Tracer"/> used for Telemetry.</param>
        public ActivityFilterSpecification(TracerFactory tracerFactory = null)
        {
            _tracerFactory = tracerFactory;
        }

        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(new ActivityFilter<T>(_tracerFactory));
        }

        IEnumerable<ValidationResult> ISpecification.Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
