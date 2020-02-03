using GiG.Core.Benchmarks.Orleans.Streams.Contracts;
using GiG.Core.Benchmarks.Orleans.Streams.Models;
using Orleans;
using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.Streams.Grains
{
    public abstract class ProducerGrain : Grain, IProducerGrain
    {
        private IAsyncStream<Message> _stream;

        private readonly string _providerName;

        protected ProducerGrain(string providerName)
        {
            _providerName = providerName;
        }

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(_providerName);
            _stream = streamProvider.GetStream<Message>(this.GetPrimaryKey(), Constants.MessageNamespace);

            await base.OnActivateAsync();
        }

        public async Task ProduceAsync(string header, string body)
        {
            var message = new Message()
            {
                Header = header,
                Body = body
            };

            await _stream.OnNextAsync(message);
        }
    }
}
