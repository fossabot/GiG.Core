FROM igcproget.igc.zone/gig-common-docker/dotnet/runtime:3.1.1-0.1.0.5 AS base

FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.1.101-0.1.0.5 AS build

COPY ["samples/Data/src/GiG.Core.Data.Migration.Sample/GiG.Core.Data.Migration.Sample.csproj", "samples/Data/src/GiG.Core.Data.Migration.Sample/"]
RUN dotnet restore "samples/Data/src/GiG.Core.Data.Migration.Sample/GiG.Core.Data.Migration.Sample.csproj"
COPY . .
RUN dotnet build /p:Version=$VERSION -c Release 

WORKDIR "/sln/artifacts/nugets"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.artifacts.list.txt"]

FROM build AS publish
WORKDIR "/sln/samples/Data/src/GiG.Core.Data.Migration.Sample"
RUN dotnet publish "GiG.Core.Data.Migration.Sample.csproj" -c Release -o /app/publish --no-build
WORKDIR "/app/publish"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.app.list.txt", "checksum.app.txt"]

FROM publish AS push
ENTRYPOINT ["/scripts/push.sh"]

FROM base AS final
COPY --from=publish /app/publish .
COPY --from=publish /app/checksum ./checksum
ENV InfoManagement__Checksum__FilePath=/checksum/checksum.app.txt
ENTRYPOINT ["dotnet", "GiG.Core.Data.Migration.Sample.dll"]