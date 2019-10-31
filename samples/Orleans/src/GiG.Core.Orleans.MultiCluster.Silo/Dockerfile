FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.0-runtime-0.1.0.14.c3386f2 AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-0.1.0.14.c3386f2 AS build

COPY ["samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Silo/GiG.Core.Orleans.MultiCluster.Silo.csproj", "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Silo/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Silo/GiG.Core.Orleans.MultiCluster.Silo.csproj"
COPY . .
WORKDIR "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Silo/"
RUN dotnet build "GiG.Core.Orleans.MultiCluster.Silo.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Orleans.MultiCluster.Silo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.MultiCluster.Silo.dll"]