# GiG.Core.Web.Mock

This Library provides a MockStartupBase class to be used for Testing.

This Base class configures the following in `ConfigureServices(IServiceCollection services)`:
 * AddControllers()
 * ConfigureForwardedHeaders()
 * ConfigureApiBehaviorOptions()
 * AddRouting()
 * AddMockRequestContextAccessor()
 * AddMockCorrelationAccessor()
 * AddMockTenantAccessor()

And the following in `Configure(IApplicationBuilder app)`:
 * UseForwardedHeaders()
 * UsePathBaseFromConfiguration()
 * UseRouting()
 * UseFluentValidationMiddleware()
 * UseEndpoints(endpoints => { endpoints.MapControllers(); })

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