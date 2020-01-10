﻿using GreenPipes;
using MassTransit;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <summary>
    /// Gets the current Activity Id and sets it in the message header.
    /// </summary>
    internal class ActivtyFilter<T> : IFilter<T> where T : class, PipeContext
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("activityId");
        }

        /// <summary>
        /// Send is called for each context that is sent through the pipeline.
        /// </summary>
        /// <param name="context">The context sent through the pipeline.</param>
        /// <param name="next">The next filter in the pipe, must be called or the pipe ends here.</param>
        public async Task Send(T context, IPipe<T> next)
        {
            // add the current Activity Id to the headers
            var publishcontext = context.GetPayload<PublishContext>();
            publishcontext.Headers.Set("CorrelationId", System.Diagnostics.Activity.Current.Id);

            // call the next filter in the pipe
            await next.Send(context);
        }
    }
}
