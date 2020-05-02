using dotnet_etcd;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/etcd")]
    [ApiVersion("1")]
    public class EtcdController : ControllerBase
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly EtcdClient _etcdClient;

        public EtcdController(IDataProvider<string> dataProvider,
            IOptions<EtcdProviderOptions> etcdProviderOptions)
        {
            _dataProvider = dataProvider;

            _etcdClient = new EtcdClient(etcdProviderOptions.Value.ConnectionString, etcdProviderOptions.Value.Port,
                etcdProviderOptions.Value.Username, etcdProviderOptions.Value.Password, etcdProviderOptions.Value.CaCertificate,
                etcdProviderOptions.Value.ClientCertificate, etcdProviderOptions.Value.ClientKey,
                etcdProviderOptions.Value.IsPublicRootCa);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<string>> Get([FromRoute, Required] string key)
        {
            var value = await _dataProvider.GetAsync(key);

            return Ok(value);
        }

        [HttpGet("{key}/duration")]
        public async Task<ActionResult<string>> GetWithDuration([FromRoute, Required] string key)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var value = await _dataProvider.GetAsync(key);
            timer.Stop();

            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return Ok(value);
        }

        [HttpGet("{key}/length")]
        public async Task<ActionResult<string>> GetLength([FromRoute, Required] string key)
        {
            var value = await _dataProvider.GetAsync(key);

            return Ok(value.Length);
        }

        [HttpGet("{key}/length/duration")]
        public async Task<ActionResult<string>> GetLengthWithDuration([FromRoute, Required] string key)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var value = await _dataProvider.GetAsync(key);
            timer.Stop();

            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return Ok(value.Length);
        }

        [HttpPost("{key}")]
        public async Task<ActionResult> Post([FromRoute, Required] string key, [FromBody, Required] ValueModel model)
        {
            var value = Convert.FromBase64String(model.DataBase64);
            await _dataProvider.WriteAsync(Encoding.UTF8.GetString(value), key);

            return NoContent();
        }

        [HttpPost("{key}/duration")]
        public async Task<ActionResult> PostWithDuration([FromRoute, Required] string key, [FromBody, Required] ValueModel model)
        {
            var value = Convert.FromBase64String(model.DataBase64);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            await _dataProvider.WriteAsync(Encoding.UTF8.GetString(value), key);
            timer.Stop();

            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return NoContent();
        }

        [HttpGet("prefix/{keyPrefix}")]
        public async Task<ActionResult<IEnumerable<KeyValuePair<string, string>>>> GetRange([FromRoute, Required] string keyPrefix)
        {
            var values = await _etcdClient.GetRangeAsync(keyPrefix);
            var list = values.Kvs.Select(x => new KeyValuePair<string, string>(x.Key.ToStringUtf8(), x.Value.ToStringUtf8()));

            return Ok(list);
        }

        [HttpGet("prefix/{keyPrefix}/duration")]
        public async Task<ActionResult<IEnumerable<KeyValuePair<string, string>>>> GetRangeWithDuration([FromRoute, Required] string keyPrefix)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var values = await _etcdClient.GetRangeAsync(keyPrefix);
            var list = values.Kvs.Select(x => new KeyValuePair<string, string>(x.Key.ToStringUtf8(), x.Value.ToStringUtf8()));
            timer.Stop();

            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return Ok(list);
        }

        [HttpGet("prefix/{keyPrefix}/length")]
        public async Task<ActionResult<string>> GetRangeLength([FromRoute, Required] string keyPrefix)
        {
            var values = await _etcdClient.GetRangeAsync(keyPrefix);
            var valuesLength = values.Kvs.Select(x => x.Value.ToStringUtf8().Length).Sum();

            return Ok(valuesLength);
        }

        [HttpGet("prefix/{keyPrefix}/length/duration")]
        public async Task<ActionResult<string>> GetRangeLengthWithDuration([FromRoute, Required] string keyPrefix)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var values = await _etcdClient.GetRangeAsync(keyPrefix);
            var valuesLength = values.Kvs.Select(x => x.Value.ToStringUtf8().Length).Sum();
            timer.Stop();

            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return Ok(valuesLength);
        }
    }
}