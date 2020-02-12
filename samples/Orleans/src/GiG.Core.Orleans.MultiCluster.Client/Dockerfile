FROM igcproget.igc.zone/gig-common-docker/dotnet/aspnet:3.1.1-0.1.0.5 AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.1.101-0.1.0.5 AS build

COPY ["samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/GiG.Core.Orleans.MultiCluster.Client.csproj", "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/GiG.Core.Orleans.MultiCluster.Client.csproj"
COPY . .
RUN dotnet build /p:Version=$VERSION -c Release 

WORKDIR "/sln/artifacts/nugets"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.artifacts.list.txt"]

FROM build AS publish
WORKDIR "/sln/samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client"
RUN dotnet publish "GiG.Core.Orleans.MultiCluster.Client.csproj" -c Release -o /app/publish --no-build
WORKDIR "/app/publish"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.app.list.txt", "checksum.app.txt"]

FROM publish AS push
ENTRYPOINT ["/scripts/push.sh"]

FROM base AS final
COPY --from=publish /app/publish .
COPY --from=publish /app/checksum ./checksum
ENV InfoManagement__Checksum__FilePath=/checksum/checksum.app.txt
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.MultiCluster.Client.dll"]