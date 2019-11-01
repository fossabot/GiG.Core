using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Security.Http
{
    /// <inheritdoc />
    public class HmacSignatureProvider : IHmacSignatureProvider
    {
        /// <inheritdoc />
        public string GetSignature(string method, string path, string body, string nonce, string secret)
        {
            return $"{secret}{nonce}{method}{path}{body}";
        }
    }
}
