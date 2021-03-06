﻿using GiG.Core.HealthChecks.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace GiG.Core.HealthChecks.Tests.Integration.Mocks
{
    internal class MockStartupWithCustomConfiguration
    {
        private readonly IConfiguration _configuration;
        
        internal const string CombinedUrl = "/hc";
        internal const string LiveUrl = "/hc/live";
        internal const string ReadyUrl = "/hc/ready";

        public MockStartupWithCustomConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureHealthChecks(_configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks();
        }
    }
}