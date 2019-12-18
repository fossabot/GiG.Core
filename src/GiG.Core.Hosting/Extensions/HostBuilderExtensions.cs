using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Host Builder Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Registers Application Metadata.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder" />.</param>
        /// <returns>The <see cref="IHostBuilder" />.</returns>
        public static IHostBuilder UseApplicationMetadata([NotNull] this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.ConfigureServices((context, services) =>
            {
                var applicationName = context.Configuration["ApplicationName"];
                ApplicationMetadata.Name = applicationName;

                ApplicationMetadata.Checksum = GetCheckSum(context.Configuration);

                services.AddApplicationMetadataAccessor();
            });
        }

        private static string GetCheckSum(IConfiguration configuration)
        {
            var checksumFilePath = "/checksum/checksum.app.txt";//configuration[""];
            var physicalFileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            var fileInfo = physicalFileProvider.GetFileInfo(checksumFilePath);

            if (!fileInfo.Exists)
                return string.Empty;
            
            using (var stream = fileInfo.CreateReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }


    }
}