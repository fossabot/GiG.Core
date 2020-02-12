using GiG.Core.Hosting;
using GiG.Core.Web.Docs.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GiG.Core.Web.Docs
{
    internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly ApiDocsOptions _apiDocsOptions;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptionsMonitor<ApiDocsOptions> apiDocsOptions)
        {
            _provider = provider;
            _apiDocsOptions = apiDocsOptions.CurrentValue;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = ApplicationMetadata.Name,
                        Description = $"{_apiDocsOptions.Description}{(description.IsDeprecated ? " [DEPRECATED]." : string.Empty)}",
                        Version = description.ApiVersion.ToString()
                    });
            }
        }
    }
}
