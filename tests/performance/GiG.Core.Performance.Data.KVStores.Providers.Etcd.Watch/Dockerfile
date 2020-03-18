FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.csproj", "tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/"]
RUN dotnet restore "tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.csproj"

# copy and publish app and libraries
COPY ./tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/ .
COPY ./NuGet.Config .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.dll"]