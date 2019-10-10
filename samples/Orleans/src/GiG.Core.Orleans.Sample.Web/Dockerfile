FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0 AS base
ENV COMPlus_ReadyToRun=0
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS build
WORKDIR /src
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