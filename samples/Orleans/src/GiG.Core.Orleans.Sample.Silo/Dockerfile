FROM igcproget.igc.zone/gig-common-docker/dotnet/aspnet:3.1.0-0.1.0.4 AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.1.100-0.1.0.4 AS build

COPY ["samples/Orleans/src/GiG.Core.Orleans.Sample.Silo/GiG.Core.Orleans.Sample.Silo.csproj", "samples/Orleans/src/GiG.Core.Orleans.Sample.Silo/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.Sample.Silo/GiG.Core.Orleans.Sample.Silo.csproj"
COPY . .
RUN dotnet build /p:Version=$VERSION -c Release 

WORKDIR "/sln/artifacts/nugets"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.artifacts.list.txt"]

FROM build AS publish
WORKDIR "/sln/samples/Orleans/src/GiG.Core.Orleans.Sample.Silo"
RUN dotnet publish "GiG.Core.Orleans.Sample.Silo.csproj" -c Release -o /app/publish
WORKDIR "/app/publish"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.app.list.txt", "checksum.app.txt"]

FROM publish AS push
ENTRYPOINT ["/scripts/push.sh"]

FROM base AS final
COPY --from=publish /app/publish .
COPY --from=publish /app/checksum /checksum
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.Sample.Silo.dll"]