using GiG.Core.Benchmarks.Orleans.Streams.Contracts;

namespace GiG.Core.Benchmarks.Orleans.Streams.Grains
{
    public class SMSProviderProducerGrain : ProducerGrain, ISMSProviderProducerGrain
    {
        public SMSProviderProducerGrain() : base(Constants.SMSProviderName)
        {

        }
    }
}