using GiG.Core.Orleans.Sample.Grains.Contracts;
using GiG.Core.Orleans.Sample.Grains.Contracts.Messages;
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
        private const decimal MinimumAmount = 10;

        private IPublishEndpoint _publishEndpoint;
        private readonly IPlayerInformationAccessor _playerInformationAccessor;

        public PaymentsController(IPublishEndpoint publishEndpoint, IPlayerInformationAccessor playerInformationAccessor)
        {
            _publishEndpoint = publishEndpoint;
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
        public async Task<ActionResult>Deposit(TransactionRequest request)
        {           
            if (request.Amount < MinimumAmount)
            {
                return BadRequest($"Deposit Amount must be greater than {MinimumAmount}.");
            }

            await _publishEndpoint.Publish(new PaymentTransactionMessage()
            {
                PlayerId = _playerInformationAccessor.PlayerId,
                Amount = request.Amount,
                TransactionType = PaymentTransactionType.Deposit
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
        public async Task<ActionResult> Withdraw(TransactionRequest request)
        {
            await _publishEndpoint.Publish(new PaymentTransactionMessage()
            {
                PlayerId = _playerInformationAccessor.PlayerId,
                Amount = request.Amount,
                TransactionType = PaymentTransactionType.Withdrawal
            });
            
            return Ok();
        }
    }
}