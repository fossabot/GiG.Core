FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview9 AS base
ENV COMPlus_ReadyToRun=0
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview9 AS build
ENV DOTNET_ROLL_FORWARD_ON_NO_CANDIDATE_FX=2
WORKDIR /src
COPY ["samples/Orleans/src/GiG.Core.Orleans.Sample.Client/GiG.Core.Orleans.Sample.Client.csproj", "samples/Orleans/src/GiG.Core.Orleans.Sample.Client/"]
RUN dotnet restore "samples/Orleans/src/GiG.Core.Orleans.Sample.Client/GiG.Core.Orleans.Sample.Client.csproj"
COPY . .
WORKDIR "samples/Orleans/src/GiG.Core.Orleans.Sample.Client/"
RUN dotnet build "GiG.Core.Orleans.Sample.Client.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GiG.Core.Orleans.Sample.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Orleans.Sample.Client.dll"]