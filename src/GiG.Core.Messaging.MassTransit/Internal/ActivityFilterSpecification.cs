using GreenPipes;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    internal class ActivityFilterSpecification<T> : IPipeSpecification<T>
        where T : class, PipeContext
    {
        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(new ActivityFilter<T>());
        }

        IEnumerable<ValidationResult> ISpecification.Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
