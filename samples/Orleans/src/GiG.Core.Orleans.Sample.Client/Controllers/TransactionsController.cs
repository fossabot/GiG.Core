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
    public class TransactionsController : ControllerBase
    {
        private const decimal MinimumAmount = 10;

        private readonly IClusterClient _clusterClient;
        private readonly IPlayerInformationAccessor _playerInformationAccessor;

        public TransactionsController(IClusterClient clusterClient, IPlayerInformationAccessor playerInformationAccessor)
        {
            _clusterClient = clusterClient;
            _playerInformationAccessor = playerInformationAccessor;
        }

        /// <summary>
        /// Gets the Current Balance
        /// </summary>
        /// <returns></returns>
        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> Get()
        {
            var balance = await _clusterClient.GetGrain<ITransactionGrain>(_playerInformationAccessor.PlayerId.Value).GetBalance();

            return Ok(balance);
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

            var balance = await _clusterClient.GetGrain<ITransactionGrain>(_playerInformationAccessor.PlayerId.Value).Deposit(request.Amount);

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
            var grain = _clusterClient.GetGrain<ITransactionGrain>(_playerInformationAccessor.PlayerId.Value);

            if (request.Amount > await grain.GetBalance())
            {
                return BadRequest("Withdraw Amount must be smaller or equal to the Balance.");
            }

            var balance = await grain.Withdraw(request.Amount);

            return Ok(balance);
        }
    }
}