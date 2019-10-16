using Orleans.Streams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Context.Orleans.Streams
{
    public interface IEventPublisher<T>
    {
        Task PublishEventAsync(T eventMessage, StreamSequenceToken token = null);
        Task PublishEventAsync(T eventMessage, Dictionary<string, string> requestContext, StreamSequenceToken token = null);
        void SetAsyncStream(IAsyncStream<T> asyncStream);
    }
}