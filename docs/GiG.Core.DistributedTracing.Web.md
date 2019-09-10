# GiG.Core.DistributedTracing.Web

This Library provides an API to register Distributed Tracing for your application.

## Basic Usage

Add the below to your Startup class and this will register the Correlation Id context accessor and the Correlation Id middleware.


```csharp

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddCorrelationId();
	}


	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseCorrelationId();
	}

```