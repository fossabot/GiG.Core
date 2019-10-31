using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Http.Contracts
 {
     public interface IWalletsClient
     {
         [Get("/balance")]
         Task<HttpResponseMessage> GetBalanceAsync([Header("player-id")] string playerId);
     }
 }