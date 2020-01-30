# GiG.Core.Data.Migration.Evolve

This Library provides an API to perform Database Migrations using SQL Scripts.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will perform the Data Migration, by executing the scripts in the default folders (Scripts and Scripts.{Environment})
 
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