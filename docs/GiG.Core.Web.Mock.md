# GiG.Core.Web.Mock

This Library provides a MockStartupBase class to be used for Testing.

This Base class registers the following:

```csharp
public virtual void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().AddFluentValidation();
    services.ConfigureForwardedHeaders();
    services.ConfigureApiBehaviorOptions();
    services.AddRouting();

    services.AddMockRequestContextAccessor();
    services.AddMockCorrelationAccessor();
    services.AddMockTenantAccessor();
}

/// <summary>
/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
/// </summary>
/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
public virtual void Configure(IApplicationBuilder app)
{
    app.UseForwardedHeaders();
    app.UsePathBaseFromConfiguration();
    app.UseCorrelation();
    app.UseRouting();
    app.UseFluentValidationMiddleware();
    
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
}
```

## Basic Usage

If the above registrations are enough for your needs, all you need is to create the following MockStartup class.
```csharp
internal class MockStartup : MockStartupBase { }
```
Then, attach it to your WebHost:
```csharp
public MyIntegrationTests()
{
    _server = new TestServer(new WebHostBuilder()
        .UseStartup<MockStartup>());
}
```

## Adding configurations

Adding new Configurations can be done by overriding the method, and adding them before the _base_ call.

```csharp
internal class MockStartup : MockStartupBase
{
    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddRequestContextAccessor();
        base.ConfigureServices(services);
    }
}
```