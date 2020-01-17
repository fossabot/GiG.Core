using MassTransit;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <inheritdoc />
    internal class MassTransitConsumerActivityObserver : IConsumeObserver
    {
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
            var parentActivityId = context.Headers.Get<string>(Constants.ActivityIdHeader, string.Empty);
            var activity = new System.Diagnostics.Activity("ConsumeMessage");
            if (!string.IsNullOrEmpty(parentActivityId))
            {
                activity.SetParentId(parentActivityId);
            }

            activity.Start();
            
            return Task.CompletedTask;
        }
    }
}
