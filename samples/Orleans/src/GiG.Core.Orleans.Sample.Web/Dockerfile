FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.0-aspnet-0.1.0.14.c3386f2 AS base
EXPOSE 8080

FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-0.1.0.14.c3386f2 AS build

COPY ["samples/Orleans/src/GiG.Core.Orleans.Sample.Web/GiG.Core.Orleans.Sample.Web.csproj", "samples/Orleans/src/GiG.Core.Orleans.Sample.Web/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.Sample.Web/GiG.Core.Orleans.Sample.Web.csproj"
COPY . .
WORKDIR "samples/Orleans/src/GiG.Core.Orleans.Sample.Web/"
RUN dotnet build "GiG.Core.Orleans.Sample.Web.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Orleans.Sample.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.Sample.Web.dll"]