using GiG.Core.Benchmarks.Orleans.Streams.Contracts;


namespace GiG.Core.Benchmarks.Orleans.Streams.Grains
{
    public class KafkaProviderProducerGrain : ProducerGrain, IKafkaProviderProducerGrain
    {
        public KafkaProviderProducerGrain() : base(Constants.KafkaProviderName)
        {

        }
    }
}