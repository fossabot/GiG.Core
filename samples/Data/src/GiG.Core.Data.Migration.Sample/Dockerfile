FROM mcr.microsoft.com/dotnet/core/runtime:3.0.0-rc1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1 AS build
WORKDIR /src
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