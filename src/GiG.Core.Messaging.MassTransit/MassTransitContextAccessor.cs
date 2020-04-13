using System.Threading;

namespace GiG.Core.Messaging.MassTransit
{
    /// <inheritdoc />
    public class MassTransitContextAccessor : IMassTransitContextAccessor
    {
        private static readonly AsyncLocal<MassTransitContext> MassTransitContextAsyncLocal = new AsyncLocal<MassTransitContext>();

        /// <inheritdoc />
        public MassTransitContext MassTransitContext
        {
            get => MassTransitContextAsyncLocal.Value;
            set => MassTransitContextAsyncLocal.Value = value;
        }
    }
}
