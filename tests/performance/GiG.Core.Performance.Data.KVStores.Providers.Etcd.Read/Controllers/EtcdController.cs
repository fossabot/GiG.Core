﻿using dotnet_etcd;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<ActionResult<string>> Get([FromRoute, Required] string key)
        {
            var value = await _etcdClient.GetValAsync(key);

            return Ok(value);
        }
        
        [HttpGet("{key}/duration")]
        public async Task<ActionResult<string>> GetWithDuration([FromRoute, Required] string key)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var value = await _etcdClient.GetValAsync(key);
            timer.Stop();
            
            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return Ok(value);
        }
        
        [HttpGet("{key}/length")]
        public async Task<ActionResult<string>> GetLength([FromRoute, Required] string key)
        {
            var value = await _etcdClient.GetValAsync(key);
            
            return Ok(value.Length);
        }
        
        [HttpGet("{key}/length/duration")]
        public async Task<ActionResult<string>> GetLengthWithDuration([FromRoute, Required] string key)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var value = await _etcdClient.GetValAsync(key);
            timer.Stop();
            
            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());

            return Ok(value.Length);
        }

        [HttpPost("{key}")]
        public async Task<ActionResult> Post([FromRoute, Required] string key, [FromBody, Required] ValueModel model)
        {
            var value = Convert.FromBase64String(model.DataBase64);
            await _etcdClient.PutAsync(key, Encoding.UTF8.GetString(value));
                
            return NoContent();
        }
        
        [HttpPost("{key}/duration")]
        public async Task<ActionResult> PostWithDuration([FromRoute, Required] string key, [FromBody, Required] ValueModel model)
        {
            var value = Convert.FromBase64String(model.DataBase64);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            await _etcdClient.PutAsync(key, Encoding.UTF8.GetString(value));
            timer.Stop();
            
            Response.Headers.Add("call-duration-ms", timer.Elapsed.Milliseconds.ToString());
                
            return NoContent();
        }
    }
}