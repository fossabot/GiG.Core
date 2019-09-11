# GiG.Core.Web.Docs

This Library provides an API to configure API Documentation.

## Basic Usage

Add the below to your Startup class and this will register API Docs. 

```csharp
	private readonly IConfiguration _configuration;

	public void ConfigureServices(IServiceCollection services)
	{
		services.ConfigureApiDocs(_configuration);
	}

	public void Configure(IApplicationBuilder app)
	{
		app.UseApiDocs();
	}
```

You can change the default values by overriding the [ApiDocsOptions](../src/GiG.Core.Web.Docs.Abstractions/ApiDocsOptions.cs) configuration options using the properties below.

| Type                   | Default  | Property Name           |
|------------------------|----------|-------------------------|
| IsEnabled              | true     | `IsEnabled`             |
| DocUrl                 | api-docs | `DocUrl`                |
| Title                  |          | `Title`                 |
| Description            |          | `LiveUrl`               |
| IsForwardedForEnabled  | true     | `IsForwardedForEnabled` |
