# GiG.Core.Http

This Library provides an API to extend the built-in `HttpClient` to set options from `configuration` and `configurationsection`.

## Basic Usage

Make use of `ConfigureHttpClient()` with `Refit` Api to configure HttpClient from configuration.

```csharp
public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
{
	services.AddRefitClient<IWalletsClient>()
		.ConfigureHttpClient(c => c.FromConfiguration(ctx.Configuration, "Wallets"));
}

```