# GiG.Core.Http.DistributedTracing

This Library provides an API to register a CorrelationIdDelegatingHandler onto the HttpClient. When using this Library your application will enrich the request with a X-Correlation-ID header.

## Basic Usage

Make use of `CorrelationIdDelegatingHandler()` when configuring your HttpClientFactory. The Handler depends on 'GiG.Core.DistributedTracing.Abstractions.ICorrelationContextAccessor'.

```csharp

var client = HttpClientFactory.CreateClient(x =>
{
    x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler(new CorrelationContextAccessor()));
    x.BaseAddress = new Uri("http://localhost");
});

```