# GiG.Core.Data.Migration

This Library provides an API to perform Database Migrations using SQL scripts.

## Basic Usage

Add the below to your Startup class to perform the Data Migation executing the scripts in the default folders (Scripts and Scripts.{Environment})
 
```csharp
    private readonly IConfiguration _configuration;
	
	public void ConfigureServices(IServiceCollection services)
	{
		 services
                .AddDbMigration(new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]))
                .AddDefaultMigrationOptions()
                .Migrate();
	}
```

You can also define specific Scripts folders as shown below. 

```csharp
    private readonly IConfiguration _configuration;
	
	public void ConfigureServices(IServiceCollection services)
	{
		 services
                .AddDbMigration(new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]))
                .AddDbMigration(connection)
				.AddLocation("CustomScripts");
                .Migrate();
	}
```

By default the results of the Migration are stored in the Changelog table. If needed you can change the name of this table as shown below.

```csharp
    private readonly IConfiguration _configuration;
	
	public void ConfigureServices(IServiceCollection services)
	{
		 services
                .AddDbMigration(new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]))
                .AddDbMigration(connection)
				.AddDefaultMigrationOptions()
				.AddMetadataTableName("customchangelog");
                .Migrate();
	}
```