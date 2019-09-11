using GiG.Core.Orleans.Abstractions.Configuration;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Consul.Extensions
{
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Configures Consul in Orleans.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection"></param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureConsul([NotNull] this ISiloBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            builder.Configure<ConsulOptions>(configurationSection);

            var consulOptions = configurationSection.Get<ConsulOptions>() ?? new ConsulOptions();

            return
                  builder.UseConsulClustering((options) =>
                  {
                      options.Address = new Uri(consulOptions.Address);
                      options.KvRootFolder = consulOptions.KvRootFolder;
                  });                
        }
    }
}
