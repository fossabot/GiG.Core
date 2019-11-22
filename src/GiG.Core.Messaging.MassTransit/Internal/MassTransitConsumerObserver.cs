using MassTransit;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <inheritdoc />
    internal class MassTransitConsumerObserver : IConsumeObserver
    {
        private readonly IMassTransitContextFactory _massTransitContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitConsumerObserver"/>.
        /// </summary>
        /// <param name="massTransitContextFactory">The <see cref="IMassTransitContextFactory"/> through which the <see cref="MassTransitContext"/> will be set.</param>
        public MassTransitConsumerObserver(IMassTransitContextFactory massTransitContextFactory)
        {
            _massTransitContextFactory = massTransitContextFactory;
        }

        /// <inheritdoc />
        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called before a message is dispatched to any consumers. Sets the <see cref="MassTransitContext"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            _massTransitContextFactory.Create(context.CorrelationId.GetValueOrDefault());
            return Task.CompletedTask;
        }
    }
}
