using GiG.Core.Web.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using GiG.Core.Hosting.Abstractions;

namespace GiG.Core.Web.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly IApplicationMetadataAccessor _applicationMetadataAccessor;

        public ManagementController(IApplicationMetadataAccessor applicationMetadataAccessor)
        {
            _applicationMetadataAccessor = applicationMetadataAccessor;
        }

        [HttpGet("version")]
        public ActionResult<string> Version()
        {
            return Ok(_applicationMetadataAccessor.Version);
        }

        [HttpGet("name")]
        public ActionResult<string> Name()
        {
            return Ok(_applicationMetadataAccessor.ApplicationName);
        }

        [HttpGet("version-info")]
        public ActionResult<string> VersionInformational()
        {
            return Ok(_applicationMetadataAccessor.InformationalVersion);
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IOptionsSnapshot<TransactionSettings> _transactionSettings;
        
        public TransactionsController(ITransactionService transactionService, IOptionsSnapshot<TransactionSettings> transactionSettings)
        {
            _transactionService = transactionService;
            _transactionSettings = transactionSettings;
        }

        /// <summary>
        /// Gets the Current Balance
        /// </summary>
        /// <returns></returns>
        [HttpGet("balance")]
        public ActionResult<decimal> Get()
        {
            return Ok(_transactionService.Balance);
        }

        /// <summary>
        /// Gets the Minimum Deposit Amount
        /// </summary>
        /// <returns></returns>
        [HttpGet("min-dep-amt")]
        
        public ActionResult<decimal> GetDepositLimit()
        {
            return Ok(_transactionSettings.Value.MinimumDepositAmount);
        }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="request">Deposit Request</param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<decimal> Deposit(TransactionRequest request)
        {
            if (request.Amount < _transactionSettings.Value.MinimumDepositAmount)
            {
                return BadRequest($"Deposit Amount must be greater than {_transactionSettings.Value.MinimumDepositAmount}.");
            }

            return Ok(_transactionService.Deposit(request.Amount));
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="request">Withdrawak Request</param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<decimal> Withdraw(TransactionRequest request)
        {
            if (request.Amount > _transactionService.Balance)
            {
                return BadRequest("Withdraw Amount must be smaller or equal to the Balance.");
            }

            return Ok(_transactionService.Withdraw(request.Amount));
        }
    }
}