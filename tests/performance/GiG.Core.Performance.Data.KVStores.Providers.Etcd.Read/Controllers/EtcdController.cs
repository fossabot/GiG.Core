using dotnet_etcd;
using Microsoft.AspNetCore.Mvc;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EtcdController : ControllerBase
    {
        private readonly EtcdClient _etcdClient;
        
        public EtcdController(EtcdClient etcdClient)
        {
            _etcdClient = etcdClient;
        }

        [HttpGet]
        public ActionResult<string> Get([FromQuery] string key)
        {
            return _etcdClient.GetVal(key);
        }

        [HttpPost]
        public ActionResult<string> Post([FromQuery] string key, [FromQuery] string value)
        {
            _etcdClient.Put(key, value);
                
            return Ok();
        }
    }
}