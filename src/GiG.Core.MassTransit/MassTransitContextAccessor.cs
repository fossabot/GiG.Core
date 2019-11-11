using System.Threading;

namespace GiG.Core.MassTransit
{ 
    /// <inheritdoc />
    public class MassTransitContextAccessor : IMassTransitContextAccessor
    {
        private readonly static AsyncLocal<MassTransitContext> _masssTransitContext = new AsyncLocal<MassTransitContext>();

        /// <inheritdoc />
        public MassTransitContext MassTransitContext
        {
            get => _masssTransitContext.Value;
            set => _masssTransitContext.Value = value;
        }
    }
}
