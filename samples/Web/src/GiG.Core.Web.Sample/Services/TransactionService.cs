using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Web.Sample.Contracts;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Web.Sample.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger _logger;
        private readonly IRequestContextAccessor _requestContextAccessor;
        private readonly IActivityContextAccessor _activityContextAccessor;

        public TransactionService(ILogger<TransactionService> logger,
            IRequestContextAccessor requestContextAccessor,
            IActivityContextAccessor activityContextAccessor)
        {
            _logger = logger;
            _requestContextAccessor = requestContextAccessor;
            _activityContextAccessor = activityContextAccessor;
        }

        public decimal Balance { get; private set; }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal Deposit(decimal amount)
        {
            _logger.LogInformation("Request Activity Name {OperationName}; Correlation {CorrelationId}; SpanId {SpanId}; TraceId {TraceId}",
                _activityContextAccessor.OperationName,
                _activityContextAccessor.CorrelationId,
                _activityContextAccessor.SpanId,
                _activityContextAccessor.TraceId);
            
            _logger.LogInformation("Request IP Address {IPAddress}", _requestContextAccessor.IPAddress);
            _logger.LogInformation("Deposit {amount}", amount);
            Balance += amount;

            return Balance;
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal Withdraw(decimal amount)
        {
            _logger.LogInformation("Withdraw {amount}", amount);
            Balance -= amount;

            return Balance;
        }
    }
}