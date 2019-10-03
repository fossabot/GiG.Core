using GiG.Core.Orleans.Sample.Client.Contracts;
using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Net;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private const decimal MinimumAmount = 10;

        private readonly IClusterClient _clusterClient;
        private readonly IPlayerInformationAccessor _playerInformationAccessor;

        public PaymentsController(IClusterClient clusterClient, IPlayerInformationAccessor playerInformationAccessor)
        {
            _clusterClient = clusterClient;
            _playerInformationAccessor = playerInformationAccessor;
        }

        /// <summary>
        /// Performs a Deposit.
        /// </summary>
        /// <param name="request">Deposit Request.</param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>>Deposit(TransactionRequest request)
        {           
            if (request.Amount < MinimumAmount)
            {
                return BadRequest($"Deposit Amount must be greater than {MinimumAmount}.");
            }

            var paymentGrain = _clusterClient.GetGrain<IPaymentGrain>(_playerInformationAccessor.PlayerId);

            await paymentGrain.DepositAsync(request.Amount);

            var walletGrain = _clusterClient.GetGrain<IWalletGrain>(_playerInformationAccessor.PlayerId);
            
            return Ok(await walletGrain.GetBalanceAsync());
        }

        /// <summary>
        /// Performs a Withdrawal.
        /// </summary>
        /// <param name="request">Withdrawal Request.</param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>> Withdraw(TransactionRequest request)
        {
            var paymentGrain = _clusterClient.GetGrain<IPaymentGrain>(_playerInformationAccessor.PlayerId);
            
            await paymentGrain.WithdrawAsync(request.Amount);

            var walletGrain = _clusterClient.GetGrain<IWalletGrain>(_playerInformationAccessor.PlayerId);

            return Ok(await walletGrain.GetBalanceAsync());
        }
    }
}