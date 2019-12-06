using System.Threading;

namespace GiG.Core.Messaging.MassTransit
{
    /// <inheritdoc />
    public class MassTransitContextAccessor : IMassTransitContextAccessor
    {
        private static readonly AsyncLocal<MassTransitContext> _massTransitContext = new AsyncLocal<MassTransitContext>();

        /// <inheritdoc />
        public MassTransitContext MassTransitContext
        {
            get => _massTransitContext.Value;
            set => _massTransitContext.Value = value;
        }
    }
}
