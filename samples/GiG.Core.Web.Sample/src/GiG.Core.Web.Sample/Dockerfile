FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview8 AS base
ENV COMPlus_ReadyToRun=0
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview8 AS build
WORKDIR /src
COPY ["samples/GiG.Core.Web.Sample/src/GiG.Core.Web.Sample/GiG.Core.Web.Sample.csproj", "samples/GiG.Core.Web.Sample/src/GiG.Core.Web.Sample/"]
RUN dotnet restore "samples/GiG.Core.Web.Sample/src/GiG.Core.Web.Sample/GiG.Core.Web.Sample.csproj"
COPY . .
WORKDIR "samples/GiG.Core.Web.Sample/src/GiG.Core.Web.Sample/"
RUN dotnet build "GiG.Core.Web.Sample.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Web.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Web.Sample.dll"]