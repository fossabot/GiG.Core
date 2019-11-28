using GiG.Core.HealthChecks.Orleans.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds the Ready and Live HealthCheck Endpoints.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IHostBuilder AddHealthCheckDependencies([NotNull] this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var healthCheckOptions = new HealthChecksOptions();
            builder.ConfigureServices((context, services) =>
            {
                healthCheckOptions = context.Configuration.GetSection(HealthChecksOptions.DefaultSectionName).Get<HealthChecksOptions>();
            });

            builder.ConfigureWebHostDefaults(webBuilder =>
             {

                 webBuilder.UseKestrel()
                 .UseUrls();

                 webBuilder.ConfigureKestrel(serverOptions =>
                 {
                     //serverOptions.ListenLocalhost(5555, listenOptions =>
                     //{
                     //    listenOptions.Protocols = HttpProtocols.Http1;
                     //});

                     serverOptions.ListenAnyIP(5555, listenOptions =>
                     {
                         listenOptions.Protocols = HttpProtocols.Http1;
                     });

                 // Set properties and call methods on options
             }).UseStartup<Startup>();

             });

            return builder;
        }

    }
}