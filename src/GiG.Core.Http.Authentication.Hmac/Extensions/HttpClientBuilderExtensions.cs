using GiG.Core.Security.Cryptography;
using GiG.Core.Authentication.Hmac.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using System.Linq;

namespace GiG.Core.Http.Authentication.Hmac.Extensions
{
    /// <summary>
    /// The <see cref="IHttpClientBuilder" /> Extensions.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="HmacDelegatingHandler" /> functionality.
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder" />.</param>        
        /// <returns>The <see cref="IHttpClientBuilder" />.</returns>
        public static IHttpClientBuilder AddHmacDelegatingHandler([NotNull]this IHttpClientBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var services = builder.Services;
            builder.AddHttpMessageHandler(x=>
            {
                var optionsAccessor = x.GetRequiredService<IOptionsSnapshot<HmacOptions>>();
                var options = optionsAccessor.Get(builder.Name);
                return new HmacDelegatingHandler(
                    Options.Create(options),
                    x.GetRequiredService<IHashProviderFactory>(),
                    x.GetRequiredService<IHmacSignatureProvider>());
            });
           
            services.TryAddSingleton<IHmacSignatureProvider, HmacSignatureProvider>();
            services.TryAddSingleton<IHashProvider, SHA256HashProvider>();
            services.TryAddSingleton<IHashProviderFactory, HashProviderFactory>();
            services.TryAddSingleton<Func<string, IHashProvider>>(x =>
                hash => x.GetServices<IHashProvider>().FirstOrDefault(sp => sp.Name.Equals(hash)));
            
            return builder;
        }

        /// <summary>
        /// Adds option provider for <see cref="HmacDelegatingHandler" /> functionality.
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder" />.</param>        
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="HmacOptions"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder" />.</returns>
        public static IHttpClientBuilder ConfigureDefaultHmacDelegatingHandlerOptionProvider([NotNull]this IHttpClientBuilder builder, [NotNull]IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.ConfigureDefaultHmacDelegatingHandlerOptionProvider(configuration.GetSection(HmacOptions.DefaultSectionName));

            return builder;
        }

        /// <summary>
        /// Adds option provider for <see cref="HmacDelegatingHandler" /> functionality.
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder" />.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="HmacOptions"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder" />.</returns>
        public static IHttpClientBuilder ConfigureDefaultHmacDelegatingHandlerOptionProvider([NotNull]this IHttpClientBuilder builder, [NotNull]IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' is incorrect.");

            builder.Services.Configure<HmacOptions>(builder.Name, configurationSection);

            return builder;
        }
    }
}
