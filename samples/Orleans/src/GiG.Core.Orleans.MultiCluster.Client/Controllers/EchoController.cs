using GiG.Core.Orleans.Client.Abstractions;
using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.MultiCluster.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EchoController : ControllerBase
    {
        private readonly IOrleansClusterClientFactory _clusterClientFactory;

        public EchoController(IOrleansClusterClientFactory clusterClientFactory)
        {
            _clusterClientFactory = clusterClientFactory;
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
            var clusterClient = _clusterClientFactory.GetClusterClient(clusterName);
            var grain = clusterClient.GetGrain<IEchoGrain>(grainId); 

            return await grain.Ping();
        }        
    }
}