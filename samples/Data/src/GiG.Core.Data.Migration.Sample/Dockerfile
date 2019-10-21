FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.0-runtime-.4.7809fed AS base

FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-.4.7809fed AS build

COPY ./NuGet.Config ./

COPY ["samples/Data/src/GiG.Core.Data.Migration.Sample/GiG.Core.Data.Migration.Sample.csproj", "samples/Data/src/GiG.Core.Data.Migration.Sample/"]
RUN dotnet restore "samples/Data/src/GiG.Core.Data.Migration.Sample/GiG.Core.Data.Migration.Sample.csproj"
COPY . .
WORKDIR "samples/Data/src/GiG.Core.Data.Migration.Sample/"
RUN dotnet build "GiG.Core.Data.Migration.Sample.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Data.Migration.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Data.Migration.Sample.dll"]