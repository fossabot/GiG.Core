﻿using GiG.Core.Extensions.DistributedTracing.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.DistributedTracing.Web.Tests.Integration
{
    public class MockStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorrelationId();
        }
    }
}
