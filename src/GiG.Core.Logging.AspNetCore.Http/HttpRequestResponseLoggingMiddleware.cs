using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System.IO;
using System.Threading.Tasks;

namespace GiG.Core.Logging.AspNetCore.Http
{
    /// <summary>
    /// The Http Request Response Logging Middleware.
    /// </summary>
    public class HttpRequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate" />.</param>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        public HttpRequestResponseLoggingMiddleware(RequestDelegate next, ILogger<HttpRequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <inheritdoc />
        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var requestStream = _recyclableMemoryStreamManager.GetStream();

            await context.Request.Body.CopyToAsync(requestStream);

            _logger.LogInformation(@"Http Request Information... 
                                    Scheme: {scheme}
                                    Host: {host}
                                    Path: {path}
                                    QueryString: {queryString}
                                    Request Body: {requestBody}",
                                    context.Request.Scheme,
                                    context.Request.Host,
                                    context.Request.Path,
                                    context.Request.QueryString,
                                    ReadStreamInChunks(requestStream));

            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            var bodyStream = context.Response.Body;

            using var responseStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.Body = responseStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(@"Http Response Information...
                                   Scheme:{scheme}
                                   Host: {host}
                                   Path: {path}
                                   QueryString: {queryString}
                                   Response Body: {text}",
                                   context.Request.Scheme,
                                   context.Request.Host,
                                   context.Request.Path,
                                   context.Request.QueryString,
                                   responseText);

            await responseStream.CopyToAsync(bodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var stringWriter = new StringWriter();

            using var streamReader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];

            int readChunkLength;

            do
            {
                readChunkLength = streamReader.ReadBlock(readChunk, 0, readChunkBufferLength);
                stringWriter.Write(readChunk, 0, readChunkLength);
            }
            while (readChunkLength > 0);

            return stringWriter.ToString();
        }
    }
}