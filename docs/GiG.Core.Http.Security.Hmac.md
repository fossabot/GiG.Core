# GiG.Core.Http.Security.Hmac

This Library provides an API to register an HmacDelegatingHandler onto the HttpClient. When using this Library, the library will inject the Hmac header to the request.

## Basic Usage

Make use of the `HmacDelegatingHandler()` when configuring your HttpClientFactory. The Handler depends on `GiG.Core.Http.Security.Hmac.IHmacOptionsProvider`,`GiG.Core.Security.Cryptography.IHashProviderFactory`,`GiG.Core.Security.Http.IHmacSignatureProvider`.

```csharp
var client = HttpClientFactory.CreateClient(x => 
{
	x.AddHttpMessageHandler(new HmacDelegatingHandler(
		new DefaultHmacOptionsProvider(options),
		new HashProviderFactory(hashFuncFactory),
		new HmacSignatureProvider()));
});

```

Make use of `AddHmacDelegatingHandler` when configuring your HttpClient.

```charp
public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
{
    services
        .AddHttpClient("Payments", 
            client => 
            {
                client.FromConfiguration(ctx.Configuration, "Payments"); 
            })
        .AddHmacDelegatingHandler()
		.ConfigureDefaultHmacDelegatingHandlerOptionProvider(_configuration);
}
```
