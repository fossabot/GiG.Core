# GiG.Core.Data.Migration

This Library provides an API to perform Database Migrations using SQL scripts.

## Basic Usage

Add the below to your Startup class and this will configure the Base Path.
 
```csharp
    private readonly IConfiguration _configuration;
	
	public void ConfigureServices(IServiceCollection services)
	{
		 services
                .AddDbMigration(new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]))
                .AddDefaultMigrationOptions()
                .AddLocation("SeedData")
                .Migrate();
	}
```