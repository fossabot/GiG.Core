FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview9 AS base
ENV COMPlus_ReadyToRun=0
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview9 AS build
WORKDIR /src
COPY ["samples/GiG.Core.Orleans.Sample.Silo/src/GiG.Core.Orleans.Sample.Silo/GiG.Core.Orleans.Sample.Silo.csproj", "samples/GiG.Core.Orleans.Sample.Silo/src/GiG.Core.Orleans.Sample.Silo/"]
RUN dotnet restore "samples/GiG.Core.Orleans.Sample.Silo/src/GiG.Core.Orleans.Sample.Client/GiG.Core.Orleans.Sample.Silo.csproj"
COPY . .
WORKDIR "samples/GiG.Core.Orleans.Sample.Silo/src/GiG.Core.Orleans.Sample.Silo/"
RUN dotnet build "GiG.Core.Orleans.Sample.Silo.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Orleans.Sample.Silo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.Sample.Silo.dll"]