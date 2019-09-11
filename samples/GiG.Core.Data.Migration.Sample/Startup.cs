using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using GiG.Core.Data.Migration.Extensions;

namespace GiG.Core.Data.Migration.Sample
{
    internal static class Startup
    { 
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            var test = new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
            test.Open();

            test.Dispose();

            services
             .AddDbMigration(new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]))
             .AddDefaultMigrationOptions()
             .Migrate();
        }
    }
}