# GiG.Core.Web.Authentication.ApiKey

This Library provides an API to register the `ApiKeyAuthenticationHandler` in `IServiceCollection`. When using this Library, requests are authenticated using the X-Api-Key header in the request.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will register the ApiKeyAuthenticationHandler.

```chsarp
public void ConfigureServices(IServiceCollection services)
{
	// Configure Api Behavior Options
	services.AddApiKeyAuthentication();
	services.ConfigureApiKeyOptions(_configuration);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	app.UseRouting();
	app.UseAuthentication();
	app.UseAuthorization();
	app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllers();
	});
}
```

## Configuration

The below table outlines the valid Configurations used to set the authorized Api Keys in the  [ApiKeyOptions](../src/GiG.Core.Web.Authentication.ApiKey.Abstractions/ApiKeyOptions.cs).

| Configuration Name    | Type                      | Required | Default Value |
|:----------------------|:--------------------------|:---------|:--------------|
| AuthorizedTenantKeys  | Dictionary<string,string> | Yes      | <null>        |

At least 1 Api Key - Tenant Id pair must be available for the configuration to be valid.

### Sample Configuration

```chsarp
{
  "Authentication": {
	"ApiKey": {
	  "AuthorizedTenantKeys": {
		"abc": "1",
			"def": "1",
			"ghi": "2"
	  }
	}
  }
}
```
With the above configuration, the Api Keys "abc", "def" and "ghi" are authorized. The first 2 keys create a claim with type "tenant_id" set to "1" in the Authentication Ticket. Similarly, the third key sets the claim value to "2". 

## Behavior

A missing X-Api-Key header will delegate the responsibility of authenticating the request to the next handler, if any, otherwise resulting in a 401 (Unauthorized) response status code. 

Any request with an X-Api-Key header value not present in the list of AuthorizedTenantKeys results in a 401 (Unauthorized) response status code.

Any request with an X-Api-Key header value present in the list of AuthorizedTenantKeys is forwarded to the respective controller for handling.