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
        /// Performs a Deposit
        /// </summary>
        /// <param name="request">Deposit Request</param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>> Deposit(TransactionRequest request)
        {
            if (request.Amount < MinimumAmount)
            {
                return BadRequest($"Deposit Amount must be greater than {MinimumAmount}.");
            }

            var balance = await _clusterClient.GetGrain<IPaymentGrain>(_playerInformationAccessor.PlayerId.Value).Deposit(request.Amount);

            return Ok(balance);
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="request">Withdrawal Request</param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>> Withdraw(TransactionRequest request)
        {
            var grain = _clusterClient.GetGrain<IPaymentGrain>(_playerInformationAccessor.PlayerId.Value);
            
            var balance = await grain.Withdraw(request.Amount);

            return Ok(balance);
        }

        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetAllAsync()
        {
            var grain = _clusterClient.GetGrain<IPaymentTransactionGrain>(_playerInformationAccessor.PlayerId.Value);

            var transactions = await grain.GetAllAsync();
            
            return Ok(transactions);
        }
    }
}