FROM mcr.microsoft.com/dotnet/core/runtime:3.0.0 AS base
ENV COMPlus_ReadyToRun=0
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS build
WORKDIR /src
COPY ["tests/performance/GiG.Core.Performance.Orleans.Streams.Consumer/GiG.Core.Performance.Orleans.Streams.Consumer.csproj", "tests/performance/GiG.Core.Performance.Orleans.Streams.Consumer/"]
RUN dotnet restore "tests/performance/GiG.Core.Performance.Orleans.Streams.Consumer/GiG.Core.Performance.Orleans.Streams.Consumer.csproj"
COPY . .
WORKDIR "tests/performance/GiG.Core.Performance.Orleans.Streams.Consumer/"
RUN dotnet build "GiG.Core.Performance.Orleans.Streams.Consumer.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Performance.Orleans.Streams.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Performance.Orleans.Streams.Consumer.dll"]