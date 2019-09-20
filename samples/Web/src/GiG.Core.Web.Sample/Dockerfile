FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1 AS base
ENV COMPlus_ReadyToRun=0
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1 AS build
WORKDIR /src
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