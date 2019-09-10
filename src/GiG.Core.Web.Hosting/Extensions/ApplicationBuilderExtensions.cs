﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Web.Hosting.Extensions
{
    /// <summary>
    /// Application Builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string DefaultPathBaseSectionName = "PATH_BASE";
     
        /// <summary>
        /// Configure the Path Base for the application using config key "PATH_BASE".
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UsePathBaseFromConfiguration([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return builder.UsePathBaseFromConfiguration(DefaultPathBaseSectionName);
        }

        /// <summary>
        /// Configure the Path Base for the application using configSectionName parameter.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <param name="configSectionName">Config key name for the path base.</param>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UsePathBaseFromConfiguration([NotNull] this IApplicationBuilder builder, [NotNull] string configSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configSectionName == null) throw new ArgumentNullException(nameof(configSectionName));
            
            var serviceProvider = builder.ApplicationServices;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var pathBase = configuration[configSectionName];
            
            if (!string.IsNullOrEmpty(pathBase))
            {
                builder.UsePathBase(pathBase);
            }

            return builder;
        }
        
    }
}