using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Messages;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Streams.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public class PaymentConsumer : IConsumer<PaymentTransactionRequested>
    {
        private readonly ILogger _logger;
        private readonly IClusterClient _client;
        private IStream<PaymentTransaction> _stream;
        private readonly IStreamFactory _streamFactory;

        public PaymentConsumer(ILogger<PaymentConsumer> logger, IClusterClient client, IStreamFactory streamFactory)
        {
            _logger = logger;
            _client = client;
            _streamFactory = streamFactory;
        }
        
        public async Task Consume(ConsumeContext<PaymentTransactionRequested> context)
        {
            _logger.LogInformation($"Consume {Enum.GetName(typeof(TransactionType), context.Message.TransactionType)} {context.Message.Amount} CorrelationId:{context.CorrelationId.GetValueOrDefault().ToString()}");
            var streamProvider = _client.GetStreamProvider(Constants.StreamProviderName);
            _stream =  _streamFactory.GetStream<PaymentTransaction>(streamProvider, context.Message.PlayerId, Constants.PaymentTransactionsStreamNamespace);

            var transactionModel = new PaymentTransaction()
            {
                Amount = context.Message.Amount,
                TransactionType = (context.Message.TransactionType == TransactionType.Deposit) ? PaymentTransactionType.Deposit : PaymentTransactionType.Withdrawal
            };

            await _stream.PublishAsync(transactionModel);
        }
    }
}