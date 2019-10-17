using Orleans.Streams;
using System;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    public interface IStreamFactory
    {
        IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string streamNameSpace);
    }
}