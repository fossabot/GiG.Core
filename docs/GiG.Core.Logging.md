@@ -0,0 +1,31 @@

# GiG.Core.Logging


This Library provides an API to register Logging using Serilog for your application.



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