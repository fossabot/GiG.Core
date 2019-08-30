using GiG.Core.Web.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Web.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("balance")]
        public ActionResult<decimal> Get()
        {
            return Ok(_transactionService.Balance);
        }

        [HttpGet("min-dep-amount")]
        public ActionResult<decimal> GetDepositLimit()
        {
            return Ok(_transactionService.Balance);
        }

        [HttpPost("deposit")]
        public ActionResult<decimal> Deposit(TransactionRequest request)
        {
            if (request.Amount < 0)
            {
                return BadRequest("Deposit Amount must be greater than 0.");
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

            if (request.Amount > _transactionService.Balance)
            {
                return BadRequest("Withdraw Amount must be smaller or equal to the Balance.");
            }

            return Ok(_transactionService.Withdraw(request.Amount));
        }
    }
}