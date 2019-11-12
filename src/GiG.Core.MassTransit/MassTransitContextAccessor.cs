using System.Threading;

namespace GiG.Core.MassTransit
{
    /// <inheritdoc />
    public class MassTransitContextAccessor : IMassTransitContextAccessor
    {
        private readonly static AsyncLocal<MassTransitContext> _massTransitContext = new AsyncLocal<MassTransitContext>();

        /// <inheritdoc />
        public MassTransitContext MassTransitContext
        {
            get => _massTransitContext.Value;
            set => _massTransitContext.Value = value;
        }
    }
}
