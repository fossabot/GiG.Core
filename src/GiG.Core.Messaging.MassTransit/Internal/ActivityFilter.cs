using GreenPipes;
using MassTransit;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <summary>
    /// Gets the current Activity Id and sets it in the message header.
    /// </summary>
    internal class ActivityFilter<T> : IFilter<T> where T : class, PipeContext
    {
        private readonly Tracer _tracer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tracerFactory">The <see cref="TracerFactory"/> used to get the <see cref="Tracer"/> used for Telemetry.</param>
        public ActivityFilter(TracerFactory tracerFactory = null)
        {
            _tracer = tracerFactory?.GetTracer(Constants.TracerName);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("ActivityId");
        }

        /// <summary>
        /// Send is called for each context that is sent through the pipeline.
        /// </summary>
        /// <param name="context">The context sent through the pipeline.</param>
        /// <param name="next">The next filter in the pipe, must be called or the pipe ends here.</param>
        public async Task Send(T context, IPipe<T> next)
        {
            var publishingActivity = new Activity(Constants.PublishActivityName);
            publishingActivity.Start();

            var span = _tracer?.StartSpanFromActivity(Constants.SpanPublishOperationNamePrefix, publishingActivity, SpanKind.Producer);

            // add the current Activity Id to the headers
            var publishContext = context.GetPayload<PublishContext>();
            publishContext.Headers.Set(Constants.ActivityIdHeader, publishingActivity.Id);

            // call the next filter in the pipe
            await next.Send(context);

            publishingActivity.Stop();
            span?.End();
        }
    }
}
