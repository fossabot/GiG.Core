using Orleans.Streams;
using System;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// Stream Factory used to instantiate stream publishers. 
    /// </summary>
    public interface IStreamFactory
    {
        /// <summary>
        /// Returns a stream publisher from the passed provider.
        /// </summary>
        /// <param name="streamProvider">The <see cref="IStreamProvider"/> from which to create the stream.</param>
        /// <param name="streamId">The stream to publish to.</param>
        /// <param name="streamNameSpace">The stream namespace on which the messages are published.</param>
        /// <typeparam name="TMessage">The type to which the message is serialized.</typeparam>
        /// <returns>An implementation of an stream publisher.</returns>
        IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string streamNameSpace);
    }
}