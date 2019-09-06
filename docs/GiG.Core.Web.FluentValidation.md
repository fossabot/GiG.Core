# GiG.Core.Web.FluentValidation

This Library provides an API to register the Fluent Validation Exception Middleware for your application.


## Basic Usage

Add the below to your Startup class and this will register the Fluent Validation Exception Middleware.


```csharp

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseFluentValidationMiddleware();
	}

```