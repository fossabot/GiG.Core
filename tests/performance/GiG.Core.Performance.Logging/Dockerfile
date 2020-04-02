FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
EXPOSE 8080
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./NuGet.Config .
COPY ["tests/performance/GiG.Core.Performance.Logging/GiG.Core.Performance.Logging.csproj", "tests/performance/GiG.Core.Performance.Logging/"]
RUN dotnet restore "tests/performance/GiG.Core.Performance.Logging/GiG.Core.Performance.Logging.csproj"

# Copy everything else and build
COPY . ./
WORKDIR /app/tests/performance/GiG.Core.Performance.Logging
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GiG.Core.Performance.Logging.dll"]