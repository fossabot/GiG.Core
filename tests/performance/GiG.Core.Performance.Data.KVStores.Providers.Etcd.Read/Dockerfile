FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
EXPOSE 8080
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read.csproj", "tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read/"]
RUN dotnet restore "tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read.csproj"

# Copy everything else and build
COPY . ./
COPY ./NuGet.Config .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read.dll"]