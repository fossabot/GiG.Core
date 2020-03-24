using MassTransit;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <inheritdoc />
    internal class MassTransitConsumerActivityObserver : IConsumeObserver
    {
        private readonly Tracer _tracer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tracerFactory">The <see cref="TracerFactory"/> used to get the <see cref="Tracer"/> used for Telemetry.</param>
        public MassTransitConsumerActivityObserver(TracerFactory tracerFactory = null)
        {
            _tracer = tracerFactory?.GetTracer("RabbitMQTracer");
        }

        /// <inheritdoc />
        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            _tracer?.CurrentSpan.End();

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
            var parentActivityId = context.Headers.Get(Constants.ActivityIdHeader, string.Empty);
            var activity = new System.Diagnostics.Activity(Constants.ConsumeActivityName);
            if (!string.IsNullOrEmpty(parentActivityId))
            {
                activity.SetParentId(parentActivityId);
            }

            activity.Start();
            _tracer?.StartSpanFromActivity(Constants.SpanConsumeOperationNamePrefix, activity, SpanKind.Consumer);
            
            return Task.CompletedTask;
        }
    }
}
