![Alt text](gig-core.png)

The latest major release is [GiG Core 2.0](release-notes/2.0/2.0.0.md).

<!-- Currently just placeholder. Need to be updated once we get the pipeline up and running -->
[![Build status](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master)](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master) 
[![NuGet](https://img.shields.io/nuget/v/GiG.Core.svg)](https://nuget.org/packages/GiG.Core)


## Docs

* Context    
    * [GiG.Core.Context.Orleans](docs/GiG.Core.Context.Orleans.md)
    * [GiG.Core.Context.Web](docs/GiG.Core.Context.Web.md)
    
* Data Migration
    * [GiG.Core.Data.Migration](docs/GiG.Core.Data.Migration.md)

* Distributed Tracing
    * [GiG.Core.DistributedTracing.Web](docs/GiG.Core.DistributedTracing.Web.md)

* Health Checks
    * [GiG.Core.HealthChecks](docs/GiG.Core.HealthChecks.md)

* Hosting
    * [GiG.Core.Hosting](docs/GiG.Core.Hosting.md)

* HTTP Client
    * [GiG.Core.Http](docs/GiG.Core.Http.md)
    * [GiG.Core.Http.DistributedTracing](docs/GiG.Core.Http.DistributedTracing.md)
	* [GiG.Core.Http.MultiTenant](docs/GiG.Core.Http.MultiTenant.md)
		
* Logging
    * [GiG.Core.Logging](docs/GiG.Core.Logging.md)
    * [GiG.Core.Logging.All](docs/GiG.Core.Logging.All.md)
    * [GiG.Core.Logging.Enrichers.MultiTenant](docs/GiG.Core.Logging.Enrichers.MultiTenant.md)

* Multi Tenancy
    * [GiG.Core.MultiTenant.Web](docs/GiG.Core.MultiTenant.Web.md)

* Orleans
    * Client - [GiG.Core.Orleans.Client](docs/GiG.Core.Orleans.Client.md)
    * Silo - [GiG.Core.Orleans.Silo](docs/GiG.Core.Orleans.Silo.md)

* Orleans Clustering
	* Clustering - [GiG.Core.Orleans.Clustering](docs/GiG.Core.Orleans.Clustering.md)
    * Consul - [GiG.Core.Orleans.Clustering.Consul](docs/GiG.Core.Orleans.Clustering.Consul.md)
    * Kubernetes - [GiG.Core.Orleans.Clustering.Kubernetes](docs/GiG.Core.Orleans.Clustering.Kubernetes.md)

* Orleans Storage
	* Npgsql - [GiG.Core.Orleans.Storage.Npgsql](docs/GiG.Core.Orleans.Storage.Npgsql.md)

* Web
    * Docs - [GiG.Core.Web.Docs](docs/GiG.Core.Web.Docs.md)
    * Hosting - [GiG.Core.Web.Hosting](docs/GiG.Core.Web.Hosting.md)
    * Validation - [GiG.Core.Web.FluentValidation](docs/GiG.Core.Web.FluentValidation.md)


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