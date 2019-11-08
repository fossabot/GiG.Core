FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.0-aspnet-0.1.0.15.e3885ad AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-0.1.0.15.e3885ad AS build

COPY ["samples/Web/src/GiG.Core.Web.Sample/GiG.Core.Web.Sample.csproj", "samples/Web/src/GiG.Core.Web.Sample/"]
RUN dotnet restore "samples/Web/src/GiG.Core.Web.Sample/GiG.Core.Web.Sample.csproj"
COPY . .
WORKDIR "samples/Web/src/GiG.Core.Web.Sample/"
RUN dotnet build "GiG.Core.Web.Sample.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Web.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Web.Sample.dll"]