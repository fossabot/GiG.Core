using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Sample.Http.Contracts;
using GiG.Core.Orleans.Sample.Web.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Http.Services
{
    public class SampleService : IHostedService
    {
        private readonly IWalletsClient _walletsClient;
        private readonly IPaymentsClient _paymentsClient;
        private readonly ILogger<SampleService> _logger;

        public SampleService(IWalletsClient walletsClient, IPaymentsClient paymentsClient,
            ILogger<SampleService> logger)
        {
            _walletsClient = walletsClient;
            _paymentsClient = paymentsClient;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var playerId = Guid.NewGuid().ToString();
            var transactionRequest = new TransactionRequest() {Amount = 50};

            var paymentsResponse = await _paymentsClient.DepositAsync(playerId, transactionRequest);
            var paymentsCorrelationId = paymentsResponse.Headers.GetValues(Constants.Header).First();

            _logger.LogInformation("Deposit: {depositAmount}; Correlation ID: {paymentsCorrelationId}",
                transactionRequest.Amount, paymentsCorrelationId);

            var walletsResponse = await _walletsClient.GetBalanceAsync(playerId);

            var balance = await walletsResponse.Content.ReadAsStringAsync();
            var walletsCorrelationId = walletsResponse.Headers.GetValues(Constants.Header).First();

            _logger.LogInformation("Balance: {balance}; Correlation ID: {walletsCorrelationId}",
                balance, walletsCorrelationId);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}