﻿using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;

namespace GiG.Core.Orleans.Client.Extensions
{
    /// <summary>
    /// Service Collection Extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Creates and registers a new <see cref="IClusterClient"/> with default options.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureClient">The configuration which will be used to set the options for the client.</param>
        public static IServiceCollection AddClusterClient(this IServiceCollection services, Action<ClientBuilder> configureClient)
        {
            var builder = new ClientBuilder();
            
            configureClient?.Invoke(builder);
            
            return services.AddSingleton(builder.BuildAndConnect());
        }
    }
}