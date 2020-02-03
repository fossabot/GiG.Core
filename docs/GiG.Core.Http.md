# GiG.Core.Http

This Library provides an API to create an `HttpClient` without using IOC.  Alternatively you can use the built-in `IHttpClientFactory` found in `Microsoft.Extensions.Http` when `ServiceCollection` can be used.  It also provides extensions to configure the client from `IConfiguration` and `IConfigurationSection`.

## Basic Usage without IoC

Make use of `Create()` factory method to initialise a new `HttpClient`. You can also use the builder to configure the HTTP Client.

```csharp
var client = HttpClientFactory.Create(x =>
{
    x.AddDelegatingHandler(new LoggingDelegatingHandler());
    x.AddDelegatingHandler(new CorrelationIdDelegatingHandler(new CorrelationContextAccessor()));
    x.Options.WithBaseAddress(new Uri("http://localhost"));
});
```

You can also use the `GetOrAdd()` factory method to create a single instance of `HttpClient`.  Make sure that you do not dispose the `HttpClient` manually.

```csharp
var client = HttpClientFactory.GetOrAdd<PaymentClient>(x =>
{
    x.AddDelegatingHandler(new LoggingDelegatingHandler());
    x.AddDelegatingHandler(new CorrelationIdDelegatingHandler(new CorrelationContextAccessor()));
    x.Options.WithBaseAddress(new Uri("http://localhost"));
});
```
or using named instance

```csharp
var client = HttpClientFactory.GetOrAdd("Payments", x =>
{
    x.AddDelegatingHandler(new LoggingDelegatingHandler());
    x.AddDelegatingHandler(new CorrelationIdDelegatingHandler(new CorrelationContextAccessor()));
    x.Options.WithBaseAddress(new Uri("http://localhost"));
});
```

## Basic Usage using IoC

It is suggested that you use the built-in `IHttpClientFactory` when possible which is found in `Microsoft.Extensions.Http` when `ServiceCollection` can be used.  The extension to configure the client from `IConfiguration` can be used as below.
**Note**: The `FromConfiguration` extension can be found in the nuget package ```GiG.Core.Http.Extensions```

```csharp
services.AddHttpClient("Payments", client =>
{
    client.FromConfiguration(ctx.Configuration, "Payments");
});
```

### Configuration

The below table outlines the valid Configurations for [DefaultClientOptions](../src/GiG.Core.Http/DefaultClientOptions.cs). By default the Configuration for the provider is expected to be under the section "HttpClient". 

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| BaseUrl            | String | Yes      |               |

The below table outlines the valid Configurations for [HttpClientOptions](../src/GiG.Core.Http/HttpClientOptions.cs).

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| Url                | String | Yes      |               |
