using GreenPipes;
using MassTransit;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <summary>
    /// Gets the current Activity Id and sets it in the message header.
    /// </summary>
    internal class ActivityFilter<T> : IFilter<T> where T : class, PipeContext
    {
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
            // add the current Activity Id to the headers
            var publishContext = context.GetPayload<PublishContext>();

            var currentActivity = System.Diagnostics.Activity.Current ?? new System.Diagnostics.Activity("PublishMessage").Start();

            publishContext.Headers.Set(Constants.ActivityIdHeader, currentActivity.Id);

            // call the next filter in the pipe
            await next.Send(context);
        }
    }
}
