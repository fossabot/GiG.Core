using GiG.Core.Orleans.Streams.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GiG.Core.Orleans.Streams
{
    public static class ServiceCollectionExtensions
    {      
        public static IServiceCollection AddStreamFactory([NotNull] this IServiceCollection services)
        {
            services.TryAddSingleton<IStreamFactory, StreamFactory>();
            return services;
        }
    }
}