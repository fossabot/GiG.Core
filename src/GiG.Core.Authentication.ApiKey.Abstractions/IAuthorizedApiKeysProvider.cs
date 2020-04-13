using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Authentication.ApiKey.Abstractions
{
    /// <summary>
    /// Authorized Api Keys provider for Api Key authentication.
    /// </summary>
    public interface IAuthorizedApiKeysProvider
    {
        /// <summary>
        /// Returns the authorized valid api keys.
        /// </summary>
        /// <returns>Mapping between Api Key and Tenant Id.</returns>
        Task<IDictionary<string, string>> GetAuthorizedApiKeysAsync(string name);
    }
}
