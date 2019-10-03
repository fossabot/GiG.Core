using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<WalletTransaction>>> Get()
        {
            var playerId = _playerInformationAccessor.PlayerId;

            var transactions = await _clusterClient.GetGrain<IWalletTransactionGrain>(playerId).GetAllAsync();
            
            return Ok(transactions);
        }
    }
}