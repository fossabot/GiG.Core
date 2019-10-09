using GiG.Core.Orleans.Sample.Grains.Contracts;
using GiG.Core.Orleans.Sample.Grains.Contracts.Messages;
using MassTransit;
using Orleans;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public class PaymentConsumer : IConsumer<PaymentTransactionMessage>
    {
        private readonly IClusterClient _client;

        public PaymentConsumer(IClusterClient client)
        {
            _client = client;
        }
        
        public Task Consume(ConsumeContext<PaymentTransactionMessage> context)
        {
            
            //_client.GetStreamProvider("").GetStream<int>(Guid.Empty,"").
            var grain = _client.GetGrain<IPaymentGrain>(context.Message.PlayerId);
            
            if (context.Message.TransactionType == PaymentTransactionType.Deposit)
            {
            }
            else
            {
                
            }

            Console.WriteLine(context.Message);
            
            return Task.CompletedTask;
        }
    }
}