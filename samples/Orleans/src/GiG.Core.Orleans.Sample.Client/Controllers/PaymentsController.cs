using GiG.Core.Orleans.Sample.Client.Contracts;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Collections.Generic;
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
        /// Get all Payments.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> Get()
        {
            var playerId = _playerInformationAccessor.PlayerId;

            var transactions = await _clusterClient.GetGrain<IPaymentTransactionGrain>(playerId).GetAllAsync();

            return Ok(transactions);
        }

        /// <summary>
        /// Performs a DepositAsync.
        /// </summary>
        /// <param name="request">DepositAsync Request.</param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>>Deposit(TransactionRequest request)
        {
            var playerId = _playerInformationAccessor.PlayerId;
            
            if (request.Amount < MinimumAmount)
            {
                return BadRequest($"Deposit Amount must be greater than {MinimumAmount}.");
            }

            await _clusterClient.GetGrain<IPaymentGrain>(playerId).DepositAsync(request.Amount);

            var balance = await _clusterClient.GetGrain<IWalletGrain>(playerId).GetBalanceAsync();
            
            return Ok(balance);
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
            var playerId = _playerInformationAccessor.PlayerId;

            await _clusterClient.GetGrain<IPaymentGrain>(playerId).WithdrawAsync(request.Amount);

            var balance = await _clusterClient.GetGrain<IWalletGrain>(playerId).GetBalanceAsync();

            return Ok(balance);
        }
    }
}