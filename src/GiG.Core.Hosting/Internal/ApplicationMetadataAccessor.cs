using GiG.Core.Hosting.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace GiG.Core.Hosting.Internal
{
    /// <inheritdoc />
    internal class ApplicationMetadataAccessor : IApplicationMetadataAccessor
    {
        private readonly IConfiguration _configuration;

        /// <inheritdoc />
        public ApplicationMetadataAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public string Name => _configuration["ApplicationName"];

        /// <inheritdoc />
        public string Version => Assembly.GetEntryAssembly()?.GetName().Version.ToString();

        /// <inheritdoc />
        public string InformationalVersion => Assembly.GetEntryAssembly()?
                                                      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                                      .InformationalVersion;
    }
}