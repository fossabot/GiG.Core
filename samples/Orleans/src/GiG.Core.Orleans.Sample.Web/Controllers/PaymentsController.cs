using FluentValidation.AspNetCore;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Messages;
using GiG.Core.Orleans.Sample.Web.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Net;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IPlayerInformationAccessor _playerInformationAccessor;
        private readonly IClusterClient _clusterClient;

        public PaymentsController(IPublishEndpoint publishEndpoint, 
            IPlayerInformationAccessor playerInformationAccessor,
            IClusterClient clusterClient)
        {
            _publishEndpoint = publishEndpoint;
            _playerInformationAccessor = playerInformationAccessor;
            _clusterClient = clusterClient;
        }

        /// <summary>
        /// Performs a Deposit.
        /// </summary>
        /// <param name="request">Deposit Request.</param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult>Deposit([CustomizeValidator(RuleSet = "Deposit")] TransactionRequest request)
        {           
            await _publishEndpoint.Publish(new PaymentTransactionRequested()
            {
                PlayerId = _playerInformationAccessor.PlayerId,
                Amount = request.Amount,
                TransactionType = TransactionType.Deposit
            });
            
            return Ok();
        }

        /// <summary>
        /// Performs a Withdrawal.
        /// </summary>
        /// <param name="request">Withdrawal Request.</param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Withdraw([CustomizeValidator(RuleSet ="Withdraw")]TransactionRequest request)
        {
            var playerId = _playerInformationAccessor.PlayerId;
            var balance = await _clusterClient.GetGrain<IWalletGrain>(playerId).GetBalanceAsync();

            if (request.Amount > balance)
            {
                return BadRequest("Withdraw Amount must be smaller or equal to the Balance, and greater than 0.");
            }

            await _publishEndpoint.Publish(new PaymentTransactionRequested()
            {
                PlayerId = _playerInformationAccessor.PlayerId,
                Amount = request.Amount,
                TransactionType = TransactionType.Withdraw
            });
            
            return Ok();
        }
    }
}