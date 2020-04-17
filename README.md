# ![GiG Core](gig-core.png)

The latest major release is [GiG Core 2.0](release-notes/2.0/2.0.0.md).

[![Build status](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master)](https://img.shields.io/bitbucket/pipelines/atlassian/adf-builder-javascript/master) 
[![NuGet](https://img.shields.io/nuget/v/GiG.Core.svg)](https://nuget.org/packages/GiG.Core)

## Libraries

- [GiG.Core.ApplicationMetrics](docs/GiG.Core.ApplicationMetrics.md) - Provides an API to register application metrics configuration options.
- [GiG.Core.ApplicationMetrics.Prometheus](docs/GiG.Core.ApplicationMetrics.Prometheus.md) - Provides an API to add application metrics which can be consumed by Prometheus.
- [GiG.Core.Configuration](docs/GiG.Core.Configuration.md) - Provides an API to add external configuration via JSON file and Environment variables.
- [GiG.Core.Context.Orleans](docs/GiG.Core.Context.Orleans.md) - Provides an API to register the Request Context Accessor functionality for Orleans.
- [GiG.Core.Context.Web](docs/GiG.Core.Context.Web.md) - Provides an API to register the Request Context Accessor functionality for a web application.
- [GiG.Core.Data.KVStores](docs/GiG.Core.Data.KVStores.md) - Provides an API to register the required services needed by the KV Stores Data Providers.
- [GiG.Core.Data.KVStores.Providers.Etcd](docs/GiG.Core.Data.KVStores.Providers.Etcd.md) - Provides an API to register data providers which will read data from etcd.
- [GiG.Core.Data.KVStores.Providers.File](docs/GiG.Core.Data.KVStores.Providers.File.md) - Provides an API to register data providers which will read data from file.
- [GiG.Core.Data.Migration.Evolve](docs/GiG.Core.Data.Migration.Evolve.md) - Provides an API to perform Database Migrations using SQL Scripts.
- [GiG.Core.DistributedTracing.Activity](docs/GiG.Core.DistributedTracing.Activity.md) - Provides an API to register Distributed Tracing using System.Diagnostics.Activity.
- [GiG.Core.DistributedTracing.MassTransit](docs/GiG.Core.DistributedTracing.MassTransit.md) - Provides an API to register Distributed Tracing for a MassTransit Consumer.
- [GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger](docs/GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.md) - Provides an API to register Distributed Tracing using OpenTelemetry.
- [GiG.Core.DistributedTracing.Orleans](docs/GiG.Core.DistributedTracing.Orleans.md) - Provides an API to register Distributed Tracing for an Orleans Client.
- [GiG.Core.HealthChecks](docs/GiG.Core.HealthChecks.md) - Provides an API to register Health Checks for an application.
- [GiG.Core.HealthChecks.Orleans](docs/GiG.Core.HealthChecks.Orleans.md) - Provides an API to register Health Checks for an Orleans Silo.
- [GiG.Core.HealthChecks.Orleans.Streams.Kafka](docs/GiG.Core.HealthChecks.Orleans.Streams.Kafka.md) - Provides an API to register Orleans Kafka Streams Health Check.
- [GiG.Core.HealthChecks.AspNetCore](docs/GiG.Core.HealthChecks.AspNetCore.md) - Provides an API to register Health Check endpoints using the EndpointRouteBuilder.
- [GiG.Core.Hosting](docs/GiG.Core.Hosting.md) - Provides an API to register hosting related functionality to an application.
- [GiG.Core.Hosting.AspNetCore](docs/GiG.Core.Hosting.AspNetCore.md) - Provides an API to register an information endpoint using the EndpointRouteBuilder.
- [GiG.Core.Http](docs/GiG.Core.Http.md) - Provides an API to create or customise an `HttpClient` without using IOC.
- [GiG.Core.Http.DistributedTracing](docs/GiG.Core.Http.DistributedTracing.md) - Provides an API to register a `CorrelationIdDelegatingHandler` onto the `HttpClient`.
- [GiG.Core.Http.MultiTenant](docs/GiG.Core.Http.MultiTenant.md) - Provides an API to register a `TenantDelegatingHandler` onto the `HttpClient`
- [GiG.Core.Http.Authentication.Hmac](docs/GiG.Core.Http.Authentication.Hmac.md) - Provides an API to register an `HmacDelegatingHandler` onto the `HttpClient`.
- [GiG.Core.Logging.All](docs/GiG.Core.Logging.All.md) - Provides an API to register Logging using Serilog and multiple Sinks and Enrichers for an application.
- [GiG.Core.Logging.AspNetCore](docs/GiG.Core.Logging.AspNetCore.md) - Provides an API to register the `HttpRequestResponseLoggingMiddleware`. It is used to log Http Requests and Http Responses.
- [GiG.Core.Logging.Enrichers.ApplicationMetadata](docs/GiG.Core.Logging.Enrichers.ApplicationMetadata.md) - Provides an API to register and ApplicationMetadata Enricher for Logging when using Serilog.
- [GiG.Core.Logging.Enrichers.Context](docs/GiG.Core.Logging.Enrichers.Context.md) - Provides an API to register the Context Enricher for Logging when using Serilog.
- [GiG.Core.Logging.Enrichers.DistributedTracing](docs/GiG.Core.Logging.Enrichers.DistributedTracing.md) - Provides an API to register the Correlation Id Enricher for Logging when using Serilog.
- [GiG.Core.Logging.Sinks.Console](docs/GiG.Core.Logging.Sinks.Console.md) - Provides an API to register Logging to a Console using Serilog for an application.
- [GiG.Core.Logging.Sinks.File](docs/GiG.Core.Logging.Sinks.File.md) - Provides an API to register Logging to a File using Serilog for an application.
- [GiG.Core.Logging.Sinks.Fluentd](docs/GiG.Core.Logging.Sinks.Fluentd.md) - Provides an API to register Logging to Fluentd using Serilog for an application.
- [GiG.Core.Logging.Sinks.RabbitMQ](docs/GiG.Core.Logging.Sinks.RabbitMQ.md) - Provides an API to register Logging to RabbitMQ using Serilog for an application.
- [GiG.Core.Messaging.Avro.Schema.Generator.MSBuild](docs/GiG.Core.Messaging.Avro.Schema.Generator.MSBuild.md) - This Library provides a Code Generator that generates the Avro schema for Public Events objects.
- [GiG.Core.Messaging.Kafka](docs/GiG.Core.Messaging.Kafka.md) - This Library provides an API to register Kafka Producers, Consumers and their dependencies for an application.
- [GiG.Core.Messaging.MassTransit](docs/GiG.Core.Messaging.MassTransit.md) - Provides an API to register MassTransit related functionality to an application.
- [GiG.Core.Models](docs/GiG.Core.Models.md) - Provides a Library for generic Models.
- [GiG.Core.MultiTenant.Activity](docs/GiG.Core.MultiTenant.Activity.md) - Provides an API to register Multi Tenancy for an application through key-value pair items in the Activity Baggage.
- [GiG.Core.ObjectMapping.AutoMapper](docs/GiG.Core.ObjectMapping.AutoMapper.md) - Provides an API to register an Object Mapper using AutoMapper.
- [GiG.Core.Orleans.Client](docs/GiG.Core.Orleans.Client.md) - Provides an API to register an Orleans Client in an application.
- [GiG.Core.Orleans.Clustering](docs/GiG.Core.Orleans.Clustering.md) - Provides Extension Methods to register Orleans Silo Membership Providers.
- [GiG.Core.Orleans.Clustering.Consul](docs/GiG.Core.Orleans.Clustering.Consul.md) - Provides an API to use Consul as a Membership Provider for Orleans Silos.
- [GiG.Core.Orleans.Clustering.Kubernetes](docs/GiG.Core.Orleans.Clustering.Kubernetes.md) - Provides an API to use Kubernetes as a Membership Provider for Orleans Silos.
- [GiG.Core.Orleans.Clustering.Localhost](docs/GiG.Core.Orleans.Clustering.Localhost.md) - This Library provides an API to configure development-only clustering and listen on localhost.
- [GiG.Core.Orleans.Reminders.Npgsql](docs/GiG.Core.Orleans.Reminders.Npgsql.md) - Provides an API to register PostgreSQL as a Storage Provider for Reminders.
- [GiG.Core.Orleans.Silo](docs/GiG.Core.Orleans.Silo.md) - Provides an API to register an Orleans Silo in an application.
- [GiG.Core.Orleans.Silo.Dashboard](docs/GiG.Core.Orleans.Silo.Dashboard.md) - Provides an API to register the Orleans Silo Dashboard.
- [GiG.Core.Orleans.Storage.Npgsql](docs/GiG.Core.Orleans.Storage.Npgsql.md) - Provides an API to register PostgreSQL as a Grain Storage Provider.
- [GiG.Core.Orleans.Streams](docs/GiG.Core.Orleans.Streams.md) - Provides an API to register an Orleans Stream Factory.
- [GiG.Core.Orleans.Streams.Kafka](docs/GiG.Core.Orleans.Streams.Kafka.md) - Provides an API to register an Orleans Stream using Kafka.
- [GiG.Core.Providers.DateTime](docs/GiG.Core.Providers.DateTime.md) - This Library provides an API to register required DateTime providers.
- [GiG.Core.TokenManager](docs/GiG.Core.TokenManager.md) - This Library provides an API to register required services for the Token Manager.
- [GiG.Core.Validation.FluentValidation.MediatR](docs/GiG.Core.Validation.FluentValidation.MediatR.md) - Provides an API to register the MediatR Pipeline Behaviour which uses Fluent Validation..
- [GiG.Core.Validation.FluentValidation.Web](docs/GiG.Core.Validation.FluentValidation.Web.md) - Provides an API to register the Fluent Validation Exception Middleware in an application.
- [GiG.Core.Web.Authentication.ApiKey](docs/GiG.Core.Web.Authentication.ApiKey.md) - Provides an API to configure `ApiKeyAuthenticationHandler`
- [GiG.Core.Web.Authentication.Hmac](docs/GiG.Core.Web.Authentication.Hmac.md) - Provides an API to configure `HmacAuthenticationHandler`
- [GiG.Core.Web.Authentication.Hmac.MultiTenant](docs/GiG.Core.Web.Authentication.Hmac.MultiTenant.md) - Provides an API to configure `HmacAuthenticationHandler` for multitenancy.
- [GiG.Core.Web.Authentication.OAuth](docs/GiG.Core.Web.Authentication.OAuth.md) - Provides an API to configure OAuth2.0 as a protocol for Authentication.
- [GiG.Core.Web.Docs](docs/GiG.Core.Web.Docs.md) - Provides an API to configure API Documentation.
- [GiG.Core.Web.Docs.Authentication.ApiKey](docs/GiG.Core.Web.Docs.Authentication.ApiKey.md) - Provides an API to add ApiKey for Authentication when accessing API Documentation.
- [GiG.Core.Web.Docs.Authentication.OAuth](docs/GiG.Core.Web.Docs.Authentication.OAuth.md) - Provides an API to configure OAuth2.0 as a protocol for Authentication when accessing API Documentation.
- [GiG.Core.Web.Hosting](docs/GiG.Core.Web.Hosting.md) - Provides an API to configure `BASE_PATH` and Forwarded Headers.
- [GiG.Core.Web.Mock](docs/GiG.Core.Web.Mock.md) - Provides a MockStartupBase class to be used for Testing.

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

```sh
cd docker
docker-compose -f docker-compose.yml -f docker-compose-<Sample Name>.yml up --build
```

You can also find more samples [here](https://bitbucket.org/gaminginnovationgroup/gig.core.samples/src/master/).