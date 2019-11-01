![Alt text](gig-core.png)

The latest major release is [GiG Core 2.0](release-notes/2.0/2.0.0.md).

[![Build status](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master)](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master) 
[![NuGet](https://img.shields.io/nuget/v/GiG.Core.svg)](https://nuget.org/packages/GiG.Core)


## Libraries

- [GiG.Core.Configuration](docs/GiG.Core.Configuration.md) - Provides an API to add external configuration via JSON file and Environment variables.
- [GiG.Core.Context.Orleans](docs/GiG.Core.Context.Orleans.md) - Provides an API to register the Request Context Accessor functionality for Orleans.
- [GiG.Core.Context.Web](docs/GiG.Core.Context.Web.md) - Provides an API to register the Request Context Accessor functionality for a web application.
- [GiG.Core.Data.KVStores](docs/GiG.Core.Data.KVStores.md) - Provides an API to register the required services needed by the KV Stores Data Providers.
- [GiG.Core.Data.KVStores.Providers.FileProviders](docs/GiG.Core.Data.KVStores.Providers.FileProviders.md) - Provides an API to register data providers which will read data from file.
- [GiG.Core.Data.Migration](docs/GiG.Core.Data.Migration.md) - Provides an API to perform Database Migrations using SQL Scripts.
- [GiG.Core.DistributedTracing.Orleans](docs/GiG.Core.DistributedTracing.Orleans.md) - Provides an API to register Distributed Tracing for an Orleans Client.
- [GiG.Core.DistributedTracing.Web](docs/GiG.Core.DistributedTracing.Web.md) - Provides an API to register Distributed Tracing for a web application.
- [GiG.Core.HealthChecks](docs/GiG.Core.HealthChecks.md) - Provides an API to register Health Checks for an application.
- [GiG.Core.Hosting](docs/GiG.Core.Hosting.md) - Provides an API to register hosting related functionality to an application.
- [GiG.Core.Http](docs/GiG.Core.Http.md) - Provides an API to create or customise an `HttpClient` without using IOC.
- [GiG.Core.Http.DistributedTracing](docs/GiG.Core.Http.DistributedTracing.md) - Provides an API to register a `CorrelationIdDelegatingHandler` onto the `HttpClient`.
- [GiG.Core.Http.MultiTenant](docs/GiG.Core.Http.MultiTenant.md) - Provides an API to register a `TenantDelegatingHandler` onto the `HttpClient`
- [GiG.Core.Logging](docs/GiG.Core.Logging.md) - Provides an API to Logging for an application.
- [GiG.Core.Logging.All](docs/GiG.Core.Logging.All.md) - Provides an API to register Logging using Serilog and multiple Sinks and Enrichers for an application.
- [GiG.Core.Logging.Sinks.Console](docs/GiG.Core.Logging.Console.md) - Provides an API to register Logging to a Console using Serilog for an application.
- [GiG.Core.Logging.Sinks.File](docs/GiG.Core.Logging.Sinks.File.md) - Provides an API to register Logging to a File using Serilog for an application.
- [GiG.Core.Logging.Sinks.Fluent](docs/GiG.Core.Logging.Sinks.Fluent.md) - Provides an API to register Logging to a File using Serilog for an application.
- [GiG.Core.Logging.Enrichers.ApplicationMetadata](docs/GiG.Core.Logging.Enrichers.ApplicationMetadata.md) - Provides an API to register and ApplicationMetadata Enricher for Logging when using Serilog.
- [GiG.Core.Logging.Enrichers.Context](docs/GiG.Core.Logging.Enrichers.Context.md) - Provides an API to register the Context Enricher for Logging when using Serilog.
- [GiG.Core.Logging.Enrichers.DistributedTracing](docs/GiG.Core.Logging.Enrichers.DistributedTracing.md) - Provides an API to register the Correlation Id Enricher for Logging when using Serilog.
- [GiG.Core.Logging.Enrichers.MultiTenant](docs/GiG.Core.Logging.Enrichers.MultiTenant.md) - Provides an API to register Tenant Id Enricher for Logging when using Serilog.
- [GiG.Core.MultiTenant.Web](docs/GiG.Core.MultiTenant.Web.md) - Provides an API to register Multi Tenancy for an application.
- [GiG.Core.Orleans.Client](docs/GiG.Core.Orleans.Client.md) - Provides an API to register an Orleans Client in an application.
- [GiG.Core.Orleans.Clustering](docs/GiG.Core.Orleans.Clustering.md) - Provides Extension Methods to register Orleans Silo Membership Providers.
- [GiG.Core.Orleans.Clustering.Consul](docs/GiG.Core.Orleans.Clustering.Consul.md) - Provides an API to use Consul as a Membership Provider for Orleans Silos.
- [GiG.Core.Orleans.Clustering.Kubernetes](docs/GiG.Core.Orleans.Clustering.Kubernetes.md) - Provides an API to use Kubernetes as a Membership Provider for Orleans Silos.
- [GiG.Core.Orleans.Silo](docs/GiG.Core.Orleans.Silo.md) - Provides an API to register an Orleans Silo in an application.
- [GiG.Core.Orleans.Storage.Npgsql](docs/GiG.Core.Orleans.Storage.Npgsql.md) - Provides an API to register PostgreSQL as a Grain Storage Provider.
- [GiG.Core.Orleans.Streams](docs/GiG.Core.Orleans.Streams.md) - Provides an API to register an Orleans Stream Factory.
- [GiG.Core.Orleans.Streams.Kafka](docs/GiG.Core.Orleans.Streams.Kafka.md) - Provides an API to register an Orleans Stream using Kafka.
- [GiG.Core.Web.Docs](docs/GiG.Core.Web.Docs.md) - Provides an API to configure API Documentation.
- [GiG.Core.Web.FluentValidation](docs/GiG.Core.Web.FluentValidation.md) - Provides an API to register the Fluent Validation Exception Middleware in an application.
- [GiG.Core.Web.Hosting](docs/GiG.Core.Web.Hosting.md) - Provides an API to configure `BASE_PATH` and Forwarded Headers.


## Build

You can build the NuGet packages using one of the following methods. You should find the NuGet packages in the `artifacts` folder after the following commands are executed successfully.

### Docker (same as build server)

```sh
# Build solution
docker build . -t gig-core:publish --target publish
# Copy Artifacts
docker run -d --entrypoint /bin/true --name temp-gig-core-publish gig-core:publish
docker cp temp-gig-core-publish:/app/artifacts/nugets/ artifacts/
# Cleanup
docker rm -f temp-gig-core-publish
docker rmi gig-core:publish
```

### Dotnet CLI

```sh
dotnet build GiG.Core.sln
```

## Tests

You can run all the integration and unit tests using the following commands:

### Docker (same as build server)

```sh
# Build solution
docker build . -t gig-core:test --target test
# Copy Artifacts
docker run --rm gig-core:test
```

### Dotnet CLI

```sh
dotnet test --filter "Category=Unit|Category=Integration" GiG.Core.sln 
```

## Sample

You can run the samples using the following commands:

### Web

```sh
docker-compose -f docker-compose.yml -f docker-compose-sample-web.yml up --build
```

### Orleans

```sh
docker-compose -f docker-compose.yml -f docker-compose-sample-orleans.yml up --build
```