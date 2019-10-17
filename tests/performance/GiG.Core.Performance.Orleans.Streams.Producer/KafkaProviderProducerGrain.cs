using Constants = GiG.Core.Performance.Orleans.Streams.Contracts.Constants;

namespace GiG.Core.Performance.Orleans.Streams.Producer
{
    public class KafkaProviderProducerGrain : ProducerGrain, IKafkaProviderProducerGrain
    {
        public KafkaProviderProducerGrain() : base (Constants.KafkaProviderName)
        {

        }
    }
}