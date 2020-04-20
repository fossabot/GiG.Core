using GiG.Core.Logging.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using System.Collections.Generic;
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

        private StringBuilder _stringBuilder;
        private IList<object> _params;

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

        /// <inheritdoc />
        public async Task Invoke(HttpContext context)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                await _next(context);
                return;
            }

            var httpRequestResponseLoggingOptions = _httpRequestResponseLoggingOptionsMonitor.CurrentValue;

            await LogRequest(httpRequestResponseLoggingOptions, context);
            await LogResponse(httpRequestResponseLoggingOptions, context);
        }


        private static StringBuilder CreateStringBuilder(string httpRequestOrHttpReponse)
        {
            return new StringBuilder()
                .AppendLine(httpRequestOrHttpReponse)
                .AppendLine("Scheme: {scheme}")
                .AppendLine("Host: {host}")
                .AppendLine("Path: {path}")
                .AppendLine("QueryString: {queryString}");
        }

        private static List<object> CreateParams(HttpContext context)
        {
            return new List<object>()
            {
                context.Request.Scheme,
                context.Request.Host,
                context.Request.Path,
                context.Request.QueryString,
            };
        }

        private async Task LogRequest(HttpRequestResponseLoggingOptions httpRequestResponseLoggingOptions, HttpContext context)
        {
            var requestOptions = httpRequestResponseLoggingOptions.Request ?? new HttpRequestResponseOptions();
            if (!requestOptions.IsEnabled)
            {
                return;
            }

            context.Request.EnableBuffering();

            using var requestStream = _recyclableMemoryStreamManager.GetStream();

            await context.Request.Body.CopyToAsync(requestStream);

            _stringBuilder = CreateStringBuilder("Http Request Information...");

            _params = CreateParams(context);

            if (requestOptions.IncludeHeaders)
            {
                _stringBuilder.AppendLine("Headers: {headers}");
                _params.Add(context.Request.Headers);
            }

            if (requestOptions.IncludeBody)
            {
                _stringBuilder.AppendLine("Request Body: {requestBody}");
                _params.Add(ReadStreamInChunks(requestStream));
            }

            _logger.LogInformation(_stringBuilder.ToString(), _params.ToArray());

            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpRequestResponseLoggingOptions httpRequestResponseLoggingOptions, HttpContext context)
        {
            var responseOptions = httpRequestResponseLoggingOptions.Response ?? new HttpRequestResponseOptions();

            if (!responseOptions.IsEnabled)
            {
                await _next(context);
                return;
            }

            var bodyStream = context.Response.Body;

            using var responseStream = _recyclableMemoryStreamManager.GetStream();

            context.Response.Body = responseStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _stringBuilder = CreateStringBuilder("Http Reponse Information...");

            _params = CreateParams(context);

            if (responseOptions.IncludeHeaders)
            {
                _stringBuilder.AppendLine("Headers: {headers}");
                _params.Add(context.Request.Headers);
            }

            if (responseOptions.IncludeBody)
            {
                _stringBuilder.AppendLine("Response Body: {text}");
                _params.Add(responseText);
            }

            _logger.LogInformation(_stringBuilder.ToString(), _params.ToArray());

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