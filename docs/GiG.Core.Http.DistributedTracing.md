# GiG.Core.Http.DistributedTracing

This Library provides an API to register a `CorrelationContextDelegatingHandler` onto the `HttpClient`. When using this Library your application will enrich the request with a X-Correlation-ID header. 

## Basic Usage

Make use of `CorrelationContextDelegatingHandler()` when configuring your `HttpClientFactory`. The Handler depends on 'GiG.Core.DistributedTracing.Abstractions.ICorrelationContextAccessor'.

```csharp

var client = HttpClientFactory.CreateClient(x =>
{
    x.AddHttpMessageHandler(new CorrelationContextDelegatingHandler(new CorrelationContextAccessor()));
    x.BaseAddress = new Uri("http://localhost");
});

```

Make use of `AddCorrelationContextDelegatingHandler` when configuring your `HttpClient`.

```csharp
public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
{
      services
        .AddHttpClient("Payments", 
            client => 
            {
                client.FromConfiguration(ctx.Configuration, "Payments"); 
            })
        .AddCorrelationContextDelegatingHandler();
}

```