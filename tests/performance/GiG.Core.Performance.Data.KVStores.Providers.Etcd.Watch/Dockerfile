# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
WORKDIR /app

# Install etcdctl for debugging
USER 0
ARG ETCD_VER=v3.4.5
ARG DOWNLOAD_URL=https://storage.googleapis.com/etcd
RUN rm -f /tmp/etcd-${ETCD_VER}-linux-amd64.tar.gz
RUN rm -rf /tmp/etcd-download-test && mkdir -p /tmp/etcd-download-test
RUN curl -L ${DOWNLOAD_URL}/${ETCD_VER}/etcd-${ETCD_VER}-linux-amd64.tar.gz -o /tmp/etcd-${ETCD_VER}-linux-amd64.tar.gz
RUN tar xzvf /tmp/etcd-${ETCD_VER}-linux-amd64.tar.gz -C /tmp/etcd-download-test --strip-components=1
RUN rm -f /tmp/etcd-${ETCD_VER}-linux-amd64.tar.gz
RUN mv /tmp/etcd-download-test/etcdctl /usr/bin/
USER 1001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
COPY ./NuGet.Config .
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.csproj", "tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/"]
RUN dotnet restore "tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.csproj"

# Copy everything else and build
COPY . .
WORKDIR tests/performance/GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch/
RUN dotnet publish -c Release -o /app/out

FROM base
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.dll"]