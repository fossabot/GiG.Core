using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Web.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class WalletsController: ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly IPlayerInformationAccessor _playerInformationAccessor;

        public WalletsController(IClusterClient clusterClient, IPlayerInformationAccessor playerInformationAccessor)
        {
            _clusterClient = clusterClient;
            _playerInformationAccessor = playerInformationAccessor;
        }

        /// <summary>
        /// Gets the Current Balance.
        /// </summary>
        /// <returns></returns>
        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> Get()
        {
            var playerId = _playerInformationAccessor.PlayerId;

            var balance = await _clusterClient.GetGrain<IWalletGrain>(playerId).GetBalanceAsync();

            return Ok(balance);
        }    
    }
}