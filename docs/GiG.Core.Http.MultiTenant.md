# GiG.Core.Http.MultiTenant

This Library provides an API to register a TenantDelegatingHandler onto the HttpClient.
When using this Library your application will enrich the request with one or more X-Tenant-ID headers.

## Basic Usage

Make use of `TenantDelegatingHandler()` when configuring your HttpClientFactory. The Handler depends on 'GiG.Core.MultiTenant.Abstractions.ITenantAccessor'.


```csharp

	var client = HttpClientFactory.CreateClient(x =>
	{
		x.AddHttpMessageHandler(new TenantDelegatingHandler(new TenantAccessor()));
		x.BaseAddress = new Uri("http://localhost");
	});

```