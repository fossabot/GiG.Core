# GiG Core

The latest major release is [GiG Core 2.0](release-notes/2.0/2.0.0.md).

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
    * Consul.Client - [GiG.Core.Orleans.Clustering.Consul.Client](docs/GiG.Core.Orleans.Clustering.Consul.Client.md)
    * Consul.Silo - [GiG.Core.Orleans.Clustering.Consul.Silo](docs/GiG.Core.Orleans.Clustering.Consul.Silo.md)
    * Kubernetes.Client - [GiG.Core.Orleans.Clustering.Kubernetes.Client](docs/GiG.Core.Orleans.Clustering.Kubernetes.Client.md)
    * Kubernetes.Silo - [GiG.Core.Orleans.Clustering.Kubernetes.Silo](docs/GiG.Core.Orleans.Clustering.Kubernetes.Silo.md)

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