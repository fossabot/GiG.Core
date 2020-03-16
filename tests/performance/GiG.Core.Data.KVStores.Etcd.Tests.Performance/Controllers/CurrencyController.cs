using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Etcd.Tests.Performance.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Etcd.Tests.Performance.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class CurrencyController : ControllerBase
    {
        private readonly IDataRetriever<IEnumerable<Currency>> _dataRetriever;

        public CurrencyController(IDataRetriever<IEnumerable<Currency>> dataRetriever)
        {
            _dataRetriever = dataRetriever;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> Get()
        {
            return Ok(await _dataRetriever.GetAsync("currencies"));
        }
    }
}