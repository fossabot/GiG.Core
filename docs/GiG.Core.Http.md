# GiG.Core.Http

This Library provides an API to create an HTTP Client without using IOC.  Alternatively you can use the built-in `IHttpClientFactory` found in `Microsoft.Extensions.Http` when `ServiceCollection` can be used.

## Basic Usage

Make use of `CreateClient()` factory method to initialise an new HTTP Client.  You can also use the builder to configure the HTTP Client.


```csharp

	var client = HttpClientFactory.CreateClient(x =>
	{
		x.AddHttpMessageHandler(new LoggingDelegatingHandler());
		x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler(new CorrelationContextAccessor()));
		x.BaseAddress = new Uri("http://localhost");
	});

```