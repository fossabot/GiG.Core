using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Messages;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Streams.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;
using Constants = GiG.Core.Orleans.Sample.Contracts.Constants;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public class PaymentConsumer : IConsumer<PaymentTransactionRequested>
    {
        private readonly ILogger _logger;
        private readonly IClusterClient _client;
        private IStream<PaymentTransaction> _stream;
        private readonly IStreamFactory _streamFactory;
        private readonly IActivityContextAccessor _activityContextAccessor;

        public PaymentConsumer(ILogger<PaymentConsumer> logger, IClusterClient client, IStreamFactory streamFactory, IActivityContextAccessor activityContextAccessor)
        {
            _logger = logger;
            _client = client;
            _streamFactory = streamFactory;
            _activityContextAccessor = activityContextAccessor;
        }
        
        public async Task Consume(ConsumeContext<PaymentTransactionRequested> context)
        {
            _logger.LogInformation($"Consume {Enum.GetName(typeof(TransactionType), context.Message.TransactionType)} {context.Message.Amount} ActivityId:{_activityContextAccessor.CorrelationId}");
            var streamProvider = _client.GetStreamProvider(Constants.StreamProviderName);
            _stream =  _streamFactory.GetStream<PaymentTransaction>(streamProvider, context.Message.PlayerId, Constants.PaymentTransactionsStreamNamespace);

            var transactionModel = new PaymentTransaction
            {
                Amount = context.Message.Amount,
                TransactionType = context.Message.TransactionType == TransactionType.Deposit ? PaymentTransactionType.Deposit : PaymentTransactionType.Withdrawal
            };

            await _stream.PublishAsync(transactionModel);
        }
    }
}