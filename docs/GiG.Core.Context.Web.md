# GiG.Core.Context.Web

This Library provides an API to register the Request Context Accessor Functionality for your application.

## Basic Usage

Add the below to your Startup class and this will register the Request Context accessor.


```csharp

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRequestContext();
	}

```