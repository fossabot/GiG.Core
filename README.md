![Alt text](gig-core.png)

The latest major release is [GiG Core 2.0](release-notes/2.0/2.0.0.md).

[![Build status](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master)](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master) 
[![NuGet](https://img.shields.io/nuget/v/GiG.Core.svg)](https://nuget.org/packages/GiG.Core)


## Libraries

- [GiG.Core.Configuration](docs/GiG.Core.Configuration.md)
- [GiG.Core.Context.Orleans](docs/GiG.Core.Context.Orleans.md)
- [GiG.Core.Context.Web](docs/GiG.Core.Context.Web.md)
- [GiG.Core.Data.Migration](docs/GiG.Core.Data.Migration.md)
- [GiG.Core.DistributedTracing.Orleans](docs/GiG.Core.DistributedTracing.Orleans.md)
- [GiG.Core.DistributedTracing.Web](docs/GiG.Core.DistributedTracing.Web.md)
- [GiG.Core.HealthChecks](docs/GiG.Core.HealthChecks.md)
- [GiG.Core.Hosting](docs/GiG.Core.Hosting.md)
- [GiG.Core.Http](docs/GiG.Core.Http.md)
- [GiG.Core.Http.DistributedTracing](docs/GiG.Core.Http.DistributedTracing.md)
- [GiG.Core.Http.MultiTenant](docs/GiG.Core.Http.MultiTenant.md)
- [GiG.Core.Logging](docs/GiG.Core.Logging.md)
- [GiG.Core.Logging.All](docs/GiG.Core.Logging.All.md)
- [GiG.Core.Logging.Enrichers.ApplicationMetadata](docs/GiG.Core.Logging.Enrichers.ApplicationMetadata.md)
- [GiG.Core.Logging.Enrichers.Context](docs/GiG.Core.Logging.Enrichers.Context.md)
- [GiG.Core.Logging.Enrichers.DistributedTracing](docs/GiG.Core.Logging.Enrichers.DistributedTracing.md)
- [GiG.Core.Logging.Enrichers.MultiTenant](docs/GiG.Core.Logging.Enrichers.MultiTenant.md)
- [GiG.Core.MultiTenant.Web](docs/GiG.Core.MultiTenant.Web.md)
- [GiG.Core.Orleans.Client](docs/GiG.Core.Orleans.Client.md)
- [GiG.Core.Orleans.Clustering](docs/GiG.Core.Orleans.Clustering.md)
- [GiG.Core.Orleans.Clustering.Consul](docs/GiG.Core.Orleans.Clustering.Consul.md)
- [GiG.Core.Orleans.Clustering.Kubernetes](docs/GiG.Core.Orleans.Clustering.Kubernetes.md)
- [GiG.Core.Orleans.Silo](docs/GiG.Core.Orleans.Silo.md)
- [GiG.Core.Orleans.Storage.Npgsql](docs/GiG.Core.Orleans.Storage.Npgsql.md)
- [GiG.Core.Orleans.Streams](docs/GiG.Core.Orleans.Streams.md)
- [GiG.Core.Web.Docs](docs/GiG.Core.Web.Docs.md)
- [GiG.Core.Web.FluentValidation](docs/GiG.Core.Web.FluentValidation.md)
- [GiG.Core.Web.Hosting](docs/GiG.Core.Web.Hosting.md)


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