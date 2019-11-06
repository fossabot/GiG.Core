using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Messages;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public class PaymentConsumer : IConsumer<PaymentTransactionRequested>
    {
        private readonly ILogger _logger;
        private readonly IClusterClient _client;
        private IAsyncStream<PaymentTransaction> _stream;

        public PaymentConsumer(ILogger<PaymentConsumer> logger, IClusterClient client)
        {
            _logger = logger;
            _client = client;
        }
        
        public async Task Consume(ConsumeContext<PaymentTransactionRequested> context)
        {
            _logger.LogInformation($"Consume {Enum.GetName(typeof(TransactionType), context.Message.TransactionType)} {context.Message.Amount} CorrelationId:{context.CorrelationId.GetValueOrDefault().ToString()}");

            var streamProvider = _client.GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<PaymentTransaction>(context.Message.PlayerId, Constants.PaymentTransactionsStreamNamespace);

            var transactionModel = new PaymentTransaction()
            {
                Amount = context.Message.Amount,
                TransactionType = (context.Message.TransactionType == TransactionType.Deposit) ? PaymentTransactionType.Deposit : PaymentTransactionType.Withdrawal
            };

            await _stream.OnNextAsync(transactionModel);
        }
    }
}