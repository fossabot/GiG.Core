using GiG.Core.Logging.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiG.Core.Logging.AspNetCore
{
    /// <summary>
    /// The Http Request Response Logging Middleware.
    /// </summary>
    public class HttpRequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<HttpRequestResponseLoggingOptions> _httpRequestResponseLoggingOptionsMonitor;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate" />.</param>
        /// <param name="httpRequestResponseLoggingOptionsMonitor">The <see cref="IOptionsMonitor{HttpRequestResponseLoggingOptions}" />.</param>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        public HttpRequestResponseLoggingMiddleware(
            RequestDelegate next,
            IOptionsMonitor<HttpRequestResponseLoggingOptions> httpRequestResponseLoggingOptionsMonitor,
            ILogger<HttpRequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _httpRequestResponseLoggingOptionsMonitor = httpRequestResponseLoggingOptionsMonitor;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// Middleware Invoke method.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                await _next(context);
                return;
            }

            var httpRequestResponseLoggingOptions = _httpRequestResponseLoggingOptionsMonitor.CurrentValue;

            await LogRequest(httpRequestResponseLoggingOptions, context.Request);
            await LogResponse(httpRequestResponseLoggingOptions, context);
        }

        private static StringBuilder CreateStringBuilder(string operationName)
        {
            return new StringBuilder()
                .AppendLine(operationName)
                .AppendLine("Scheme: {scheme}")
                .AppendLine("Host: {host}")
                .AppendLine("Path: {path}")
                .AppendLine("QueryString: {queryString}");
        }

        private static List<object> CreateParams(HttpRequest request)
        {
            return new List<object>
            {
                request.Scheme,
                request.Host,
                request.Path,
                request.QueryString,
            };
        }
        
        private async Task LogRequest(HttpRequestResponseLoggingOptions httpRequestResponseLoggingOptions, HttpRequest request)
        {
            var requestOptions = httpRequestResponseLoggingOptions.Request ?? new HttpRequestResponseOptions();
            if (!requestOptions.IsEnabled)
            {
                return;
            }

            request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();

            await request.Body.CopyToAsync(requestStream);
            var stringBuilder = CreateStringBuilder("Http Request Information...");
            var @params = CreateParams(request);

            if (requestOptions.IncludeHeaders)
            {
                stringBuilder.AppendLine("Headers: {headers}");
                @params.Add(request.Headers.ToImmutableArray());
            }

            if (requestOptions.IncludeBody)
            {
                stringBuilder.AppendLine("Request Body: {requestBody}");
                @params.Add(ReadStreamInChunks(requestStream));
            }

            _logger.LogInformation(stringBuilder.ToString(), @params.ToArray());

            request.Body.Position = 0;
        }

        private async Task LogResponse(HttpRequestResponseLoggingOptions httpRequestResponseLoggingOptions, HttpContext context)
        {
            var responseOptions = httpRequestResponseLoggingOptions.Response ?? new HttpRequestResponseOptions();

            if (!responseOptions.IsEnabled)
            {
                await _next(context);

                return;
            }

            var response = context.Response;
            var bodyStream = response.Body;

            await using var responseStream = _recyclableMemoryStreamManager.GetStream();
            response.Body = responseStream;

            await _next(context);

            response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            var stringBuilder = CreateStringBuilder("Http Response Information...");
            var @params = CreateParams(context.Request);

            if (responseOptions.IncludeHeaders)
            {
                stringBuilder.AppendLine("Headers: {headers}");
                @params.Add(response.Headers.ToImmutableArray());
            }

            if (responseOptions.IncludeBody)
            {
                stringBuilder.AppendLine("Response Body: {text}");
                @params.Add(responseText);
            }

            _logger.LogInformation(stringBuilder.ToString(), @params.ToArray());

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