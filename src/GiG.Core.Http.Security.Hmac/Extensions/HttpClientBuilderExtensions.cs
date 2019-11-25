using GiG.Core.Security.Cryptography;
using GiG.Core.Security.Hmac.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using System.Linq;

namespace GiG.Core.Http.Security.Hmac.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> for <see cref="HmacDelegatingHandler"/>.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="HmacDelegatingHandler" /> functionality.
        /// </summary>
        /// <param name="httpClientBuilder">The <see cref="IHttpClientBuilder" />.</param>        
        /// <returns>The <see cref="IHttpClientBuilder" />.</returns>
        public static IHttpClientBuilder AddHmacDelegatingHandler([NotNull]this IHttpClientBuilder httpClientBuilder)
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));

            var services = httpClientBuilder.Services;
            httpClientBuilder.AddHttpMessageHandler(x=>
            {
                var optionsAccessor = x.GetRequiredService<IOptionsSnapshot<HmacOptions>>();
                var options = optionsAccessor.Get(httpClientBuilder.Name);
                return new HmacDelegatingHandler(
                    Options.Create(options),
                    x.GetRequiredService<IHashProviderFactory>(),
                    x.GetRequiredService<IHmacSignatureProvider>());
            });
           
            services.TryAddSingleton<IHmacSignatureProvider, HmacSignatureProvider>();
            services.TryAddTransient<IHashProvider, SHA256HashProvider>();
            services.TryAddSingleton<IHashProviderFactory, HashProviderFactory>();
            services.TryAddSingleton<Func<string, IHashProvider>>(x =>
                hash => x.GetServices<IHashProvider>().FirstOrDefault(sp => sp.Name.Equals(hash)));
            
            return httpClientBuilder;
        }

        /// <summary>
        /// Adds option provider for <see cref="HmacDelegatingHandler" /> functionality.
        /// </summary>
        /// <param name="httpClientBuilder">The <see cref="IHttpClientBuilder" />.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />Configuration section for hmac settings.</param>        
        /// <returns>The <see cref="IHttpClientBuilder" />.</returns>
        public static IHttpClientBuilder ConfigureDefaultHmacDelegatingHandlerOptionProvider([NotNull]this IHttpClientBuilder httpClientBuilder, [NotNull]IConfigurationSection configurationSection)
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' is incorrect.");

            httpClientBuilder.Services.Configure<HmacOptions>(httpClientBuilder.Name, configurationSection);


            return httpClientBuilder;
        }

        /// <summary>
        /// Adds option provider for <see cref="HmacDelegatingHandler" /> functionality.
        /// </summary>
        /// <param name="httpClientBuilder">The <see cref="IHttpClientBuilder" />.</param>        
        /// <param name="configuration">The <see cref="IConfiguration" />Configuration for hmac settings.</param>        
        /// <returns>The <see cref="IHttpClientBuilder" />.</returns>
        public static IHttpClientBuilder ConfigureDefaultHmacDelegatingHandlerOptionProvider([NotNull]this IHttpClientBuilder httpClientBuilder, [NotNull]IConfiguration configuration)
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            httpClientBuilder.ConfigureDefaultHmacDelegatingHandlerOptionProvider(configuration.GetSection(HmacOptions.DefaultSectionName));

            return httpClientBuilder;
        }
    }
}
