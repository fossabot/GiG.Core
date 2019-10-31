using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Web.Security.Hmac.Tests.Integration.Mocks
{
    public class MockDelegatingHandler:DelegatingHandler
    {
        private readonly HttpClient _client;

        public MockDelegatingHandler(HttpClient client)
        {
            _client = client;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var newRequest = await CloneHttpRequestMessageAsync(request, request.RequestUri.LocalPath);
            return await _client.SendAsync(newRequest, cancellationToken);
        }

        private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req, string newUrl)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, newUrl);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var ms = new MemoryStream();
            if (req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                // Copy the content headers
                if (req.Content.Headers != null)
                    foreach (var h in req.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
            }


            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}
