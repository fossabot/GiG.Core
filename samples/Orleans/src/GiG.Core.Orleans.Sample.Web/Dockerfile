FROM igcproget.igc.zone/gig-common-docker/dotnet/aspnet:3.1.0-0.1.0.4 AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.1.100-0.1.0.4 AS build

COPY ["samples/Orleans/src/GiG.Core.Orleans.Sample.Web/GiG.Core.Orleans.Sample.Web.csproj", "samples/Orleans/src/GiG.Core.Orleans.Sample.Web/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.Sample.Web/GiG.Core.Orleans.Sample.Web.csproj"
COPY . .
RUN dotnet build /p:Version=$VERSION -c Release 

WORKDIR "/sln/artifacts/nugets"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.artifacts.list.txt"]

FROM build AS publish
WORKDIR "/sln/samples/Orleans/src/GiG.Core.Orleans.Sample.Web"
RUN dotnet publish "GiG.Core.Orleans.Sample.Web.csproj" -c Release -o /app/publish --no-build
WORKDIR "/app/publish"
RUN ["/scripts/checksum.sh", "/app/checksum/", "checksum.app.list.txt", "checksum.app.txt"]

FROM publish AS push
ENTRYPOINT ["/scripts/push.sh"]

FROM base AS final
COPY --from=publish /app/publish .
COPY --from=publish /app/checksum ./checksum
ENV InfoManagement__Checksum__FilePath=/checksum/checksum.app.txt
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.Sample.Web.dll"]