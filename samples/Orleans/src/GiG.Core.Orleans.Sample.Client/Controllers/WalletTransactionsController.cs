using GiG.Core.Orleans.Sample.Client.Contracts;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletTransactionsController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly IPlayerInformationAccessor _playerInformationAccessor;

        public WalletTransactionsController(IClusterClient clusterClient, IPlayerInformationAccessor playerInformationAccessor)
        {
            _clusterClient = clusterClient;
            _playerInformationAccessor = playerInformationAccessor;
        }
        
        /// <summary>
        /// Gets the Payment Transactions.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<WalletTransaction>>> GetAllAsync()
        {
            var grain = _clusterClient.GetGrain<IWalletTransactionGrain>(_playerInformationAccessor.PlayerId.Value);

            var transactions = await grain.GetAllAsync();
            
            return Ok(transactions);
        }
    }
}