using GiG.Core.Web.Docs.Filters;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace GiG.Core.Web.Docs.Extensions
{
    internal static class SwaggerOptionsExtensions
    {
        internal static void IncludeXmlComments(this SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (string.IsNullOrEmpty(xmlPath))
            {
                throw new ApplicationException("The following property is missing from your project; <GenerateDocumentationFile>true</GenerateDocumentationFile>.");
            }

            options.IncludeXmlComments(xmlPath);
        }

        internal static void IncludeFullNameCustomSchemaId(this SwaggerGenOptions options)
        {
            options.CustomSchemaIds(x => x.FullName);
        }

        internal static void IncludeForwardedForFilter(this SwaggerGenOptions options, bool isEnabled)
        {
            if (isEnabled)
            {
                options.OperationFilter<ForwardedForOperationFilter>();
            }
        }
    }
}