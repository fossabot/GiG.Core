@@ -0,0 +1,31 @@

# GiG.Core.Extensions.Logging.All


This Library provides an API to register Logging using Serilog and muliple sinks and enrichers for your application.

When using this Library your application will log to Console and Fluentd, and will also enrich your logs with Application Metadata and CorrelationId.


## Basic Usage


Make use of ConfigureLogging() when Creating an IHostBuilder. Logging requires configuration.


```csharp

	static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(builder => builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .ConfigureLogging();
        }
    }

```