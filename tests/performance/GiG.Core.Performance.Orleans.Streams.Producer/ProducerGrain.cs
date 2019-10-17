using GiG.Core.Performance.Orleans.Streams.Contracts;
using Orleans;
using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Orleans.Streams.Producer
{
    public abstract class ProducerGrain : Grain, IProducerGrain
    {
        private IAsyncStream<Message> _stream;

        private string _providerName;

        public ProducerGrain(string providerName)
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
