FROM igcproget.igc.zone/gig-common-docker/dotnet/runtime:3.1.1-0.1.0.5 AS base

FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.1.101-0.1.0.5 AS build
COPY ["samples/Data/src/GiG.Core.Data.KVStores.Etcd.Sample/GiG.Core.Data.KVStores.Etcd.Sample.csproj", "samples/Data/src/GiG.Core.Data.KVStores.Etcd.Sample/"]
RUN dotnet restore "samples/Data/src/GiG.Core.Data.KVStores.Etcd.Sample/GiG.Core.Data.KVStores.Etcd.Sample.csproj"
COPY . .
WORKDIR "/sln/samples/Data/src/GiG.Core.Data.KVStores.Etcd.Sample"
RUN dotnet publish "GiG.Core.Data.KVStores.Etcd.Sample.csproj" -c Release -o /app/publish

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Data.KVStores.Etcd.Sample.dll"]