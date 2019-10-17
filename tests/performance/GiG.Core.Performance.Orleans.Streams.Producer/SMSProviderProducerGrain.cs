using Constants = GiG.Core.Performance.Orleans.Streams.Contracts.Constants;

namespace GiG.Core.Performance.Orleans.Streams.Producer
{
    public class SMSProviderProducerGrain : ProducerGrain, ISMSProviderProducerGrain
    {
        public SMSProviderProducerGrain() : base(Constants.SMSProviderName)
        {

        }
    }
}