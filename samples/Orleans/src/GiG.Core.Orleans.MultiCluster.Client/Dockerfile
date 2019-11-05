FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.0-aspnet-0.1.0.15.e3885ad AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-0.1.0.15.e3885ad AS build

COPY ["samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/GiG.Core.Orleans.MultiCluster.Client.csproj", "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/GiG.Core.Orleans.MultiCluster.Client.csproj"
COPY . .
WORKDIR "/src/samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client"
RUN dotnet build "GiG.Core.Orleans.MultiCluster.Client.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Orleans.MultiCluster.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.MultiCluster.Client.dll"]