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
        private readonly IClusterClient _clusterClient;

        public TransactionsController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        /// <summary>
        /// Gets the Current Balance
        /// </summary>
        /// <returns></returns>
        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> Get()
        {
            var balance = await _clusterClient.GetGrain<ITransactionGrain>("player1").GetBalance();

            return Ok(balance);
        }

        /// <summary>
        /// Gets the Minimum Deposit Amount
        /// </summary>
        /// <returns></returns>
        [HttpGet("min-dep-amt")]
        
        public ActionResult<decimal> GetDepositLimit()
        {
            return Ok(10);
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
            if (request.Amount < 10)
            {
                return BadRequest($"Deposit Amount must be greater than {10}.");
            }

            var balance = await _clusterClient.GetGrain<ITransactionGrain>("player1").Deposit(request.Amount);

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
            var grain = _clusterClient.GetGrain<ITransactionGrain>("player1");

            if (request.Amount > await grain.GetBalance())
            {
                return BadRequest("Withdraw Amount must be smaller or equal to the Balance.");
            }

            var balance = await grain.Withdraw(request.Amount);

            return Ok(balance);
        }
    }
}