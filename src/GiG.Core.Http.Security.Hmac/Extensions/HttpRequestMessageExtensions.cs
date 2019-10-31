using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace GiG.Core.Http.Security.Hmac.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static async System.Threading.Tasks.Task<string> AsSignatureStringAsync(this HttpRequestMessage httpRequest, string nonceHeader, string secret)
        {
            var body = "";
            switch (httpRequest.Method.Method.ToUpper())
            {
                case "POST":
                case "PUT":
                case "PATCH":
                    body = await httpRequest.Content.ReadAsStringAsync();
                    break;

                default:
                    body = "";
                    break;
            }
            if (!httpRequest.Headers.TryGetValues(nonceHeader, out var nonceValue))
            {
                throw new ArgumentException("Nonce value is empty", "nonceHeader");
            }
            return $"{secret}{nonceValue.FirstOrDefault()}{httpRequest.Method.ToString().ToUpper()}{httpRequest.RequestUri}{body}";
        }
    }
}
