# GiG.Core.DistributedTracing.Web

This Library provides an API to register Distributed Tracing for a web application.

## Basic Usage

The below code needs to be added to the `Program.cs`. This will register the Correlation Id context accessor and the Correlation Id middleware.

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