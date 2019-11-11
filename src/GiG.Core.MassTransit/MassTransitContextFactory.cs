using System;

namespace GiG.Core.MassTransit
{
    internal class MassTransitContextFactory : IMassTransitContextFactory
    {
        private readonly IMassTransitContextAccessor _massTransitContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitContextFactory" />.
        /// </summary>
        public MassTransitContextFactory()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitContextFactory"/>.
        /// </summary>
        /// <param name="massTransitContextAccessor">The <see cref="IMassTransitContextAccessor"/> through which the <see cref="MassTransitContext"/> will be set.</param>
        public MassTransitContextFactory(IMassTransitContextAccessor massTransitContextAccessor)
        {
            _massTransitContextAccessor = massTransitContextAccessor;
        }

        /// <inheritdoc />
        public MassTransitContext Create(Guid correlationId)
        {
            var correlationContext = new MassTransitContext(correlationId);

            if (_massTransitContextAccessor != null)
            {
                _massTransitContextAccessor.MassTransitContext = correlationContext;
            }

            return correlationContext;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_massTransitContextAccessor != null)
            {
                _massTransitContextAccessor.MassTransitContext = null;
            }
        }
    }
}
