using MassTransit;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <inheritdoc />
    internal class MassTransitConsumerActivityObserver : IConsumeObserver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitConsumerActivityObserver"/>.
        /// </summary>
        public MassTransitConsumerActivityObserver()
        {
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
            var parentId = context.Headers.Get<string>("CorrelationId");
            var activity = new System.Diagnostics.Activity("Consumer");
            activity.SetParentId(parentId);
            activity.Start();
            return Task.CompletedTask;
        }
    }
}
