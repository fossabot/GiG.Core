using dotnet_etcd;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
            return Ok(_etcdClient.GetVal(key));
        }
        
        [HttpGet("{key}/length")]
        public ActionResult<string> GetLength([FromRoute, Required] string key)
        {
            return Ok(_etcdClient.GetVal(key).Length);
        }

        [HttpPost("{key}")]
        public ActionResult Post([FromRoute, Required] string key, [FromBody, Required] ValueModel model)
        {
            var value = Convert.FromBase64String(model.DataBase64);
            _etcdClient.Put(key, Encoding.UTF8.GetString(value));
                
            return NoContent();
        }
    }
}