using GiG.Core.Context.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Web.Mock.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockRequestContextAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IRequestContextAccessor>(new MockRequestContextAccessor());

            return services;
        }
    }
}
