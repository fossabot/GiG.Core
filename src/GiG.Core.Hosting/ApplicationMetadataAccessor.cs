using GiG.Core.Hosting.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace GiG.Core.Hosting
{
    /// <inheritdoc />
    public class ApplicationMetadataAccessor : IApplicationMetadataAccessor
    {
        private readonly IConfiguration _configuration;

        public ApplicationMetadataAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public string ApplicationName => _configuration["ApplicationName"];

        /// <inheritdoc />
        public string Version => Assembly.GetEntryAssembly()?.GetName().Version.ToString();

        /// <inheritdoc />
        public string InformationalVersion => Assembly.GetEntryAssembly()?
                                                      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                                      .InformationalVersion;

    }
}