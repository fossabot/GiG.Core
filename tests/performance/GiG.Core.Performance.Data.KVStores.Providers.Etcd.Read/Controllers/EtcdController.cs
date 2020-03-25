using dotnet_etcd;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/etcd")]
    [ApiVersion("1")]
    public class EtcdController : ControllerBase
    {
        private readonly EtcdClient _etcdClient;
        
        public EtcdController(EtcdClient etcdClient)
        {
            _etcdClient = etcdClient;
        }

        [HttpGet("{key}")]
        public ActionResult<string> Get([FromRoute, Required] string key)
        {
            return _etcdClient.GetVal(key);
        }

        [HttpPost("{key}")]
        public ActionResult Post([FromRoute, Required] string key, [FromQuery, Required] string value)
        {
            _etcdClient.Put(key, value);
                
            return NoContent();
        }
    }
}