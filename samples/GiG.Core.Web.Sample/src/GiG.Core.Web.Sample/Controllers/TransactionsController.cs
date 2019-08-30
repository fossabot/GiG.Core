using GiG.Core.Web.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GiG.Core.Web.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly TransactionSettings _transactionSettings;
        
        public TransactionsController(ITransactionService transactionService, IOptionsSnapshot<TransactionSettings> transactionSettings)
        {
            _transactionService = transactionService;
            _transactionSettings = transactionSettings.Value;
        }

        [HttpGet("balance")]
        public ActionResult<decimal> Get()
        {
            return Ok(_transactionService.GetBalance());
        }

        [HttpGet("min-dep-amount")]
        public ActionResult<decimal> GetDepositLimit()
        {
            return Ok(_transactionSettings.MinimumDepositAmount);
        }

        [HttpPost("deposit")]
        public ActionResult<decimal> Deposit(TransactionRequest request)
        {
            if (request.Amount < _transactionSettings.MinimumDepositAmount)
            {
                return BadRequest($"Deposit Amount must be greater than {_transactionSettings.MinimumDepositAmount}.");
            }

            return Ok(_transactionService.Deposit(request.Amount));
        }

        [HttpPost("withdraw")]
        public ActionResult<decimal> Withdraw(TransactionRequest request)
        {
            if (request.Amount < 0)
            {
                return BadRequest("Withdraw Amount must be greater than 0.");
            }

            if (request.Amount > _transactionService.GetBalance())
            {
                return BadRequest("Withdraw Amount must be smaller or equal to the Balance.");
            }

            return Ok(_transactionService.Withdraw(request.Amount));
        }
    }
}