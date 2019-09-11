FROM mcr.microsoft.com/dotnet/core/runtime:3.0.0-preview9-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview9 AS build
WORKDIR /src
COPY ["samples/GiG.Core.Data.Migration.Sample/GiG.Core.Data.Migration.Sample.csproj", "samples/GiG.Core.Data.Migration.Sample/"]
RUN dotnet restore "samples/GiG.Core.Data.Migration.Sample/GiG.Core.Data.Migration.Sample.csproj"
COPY . .
WORKDIR "samples/GiG.Core.Data.Migration.Sample/"
RUN dotnet build "GiG.Core.Data.Migration.Sample.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Data.Migration.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Data.Migration.Sample.dll"]