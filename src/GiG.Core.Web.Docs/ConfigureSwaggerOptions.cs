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
        private readonly ApiDocsOptions _apiDocsOptions;
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IOptionsMonitor<ApiDocsOptions> apiDocsOptions, IApiVersionDescriptionProvider provider = null)
        {
            _apiDocsOptions = apiDocsOptions.CurrentValue;
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            if (_provider == null)
            {
                AddSwaggerDoc(options);

                return;
            }

            foreach (var description in _provider.ApiVersionDescriptions)
            {
                AddSwaggerDoc(options, description.GroupName, description.ApiVersion.ToString(), description.IsDeprecated);
            }
        }

        private void AddSwaggerDoc(SwaggerGenOptions options, string groupName = "v1", string apiVersion = null, bool isDeprecated = false)
        {
            options.SwaggerDoc(
                groupName,
                new OpenApiInfo
                {
                    Title = _apiDocsOptions.Title ?? ApplicationMetadata.Name,
                    Description = $"{_apiDocsOptions.Description}{(isDeprecated ? " [DEPRECATED]." : string.Empty)}",
                    Version = apiVersion
                });
        }
    }
}
