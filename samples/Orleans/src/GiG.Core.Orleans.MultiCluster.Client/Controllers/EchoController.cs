using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace GiG.Core.Orleans.MultiCluster.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EchoController : ControllerBase
    {
        //TODO: this will then be the cluster client factory
        private readonly IClusterClient _clusterClient;
        public EchoController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        /// <summary>
        /// Gets an echo response from an <see cref="IEchoGrain"/> located in a silo in the specified cluster.
        /// </summary>
        /// <param name="clusterName">The cluster name from which to request the grain.</param>
        /// <returns>The message from the grain instance.</returns>
        [HttpGet("ping")]
        public async Task<string> PingAsync([FromQuery] string clusterName)
        {
            var grainId = string.Format("{0}_echo_grain", clusterName);

            var grain = _clusterClient.GetGrain<IEchoGrain>(grainId); //TODO: the clustername will be fed to the factory to get the correct client.

            return await grain.Ping();
        }        
    }
}