using GiG.Core.Context.Abstractions;
using GiG.Core.Logging.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Logging.Tests.Integration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestContextAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IRequestContextAccessor>(new RequestContextAccessor());

            return services;
        }
    }
}
