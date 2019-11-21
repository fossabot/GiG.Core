using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    public interface IStream<in TMessage>
    {
        Task PublishAsync(TMessage message, StreamSequenceToken token = null);
    }
}