FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.0-aspnet-.4.7809fed AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-.4.7809fed AS build

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