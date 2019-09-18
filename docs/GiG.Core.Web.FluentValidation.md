# GiG.Core.Web.FluentValidation

This Library provides an API to register the Fluent Validation Exception Middleware for your application.


## Basic Usage

Add the below to your Startup class and this will register the Fluent Validation Exception Middleware.
Make sure to place the `ConfigureApiBehaviorOptions` at the end of the `ConfigureServices`.


```csharp

	public void ConfigureServices(IServiceCollection services)
    {
        // Configure Api Behavior Options
        services.ConfigureApiBehaviorOptions();
    }

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseFluentValidationMiddleware();
	}

```