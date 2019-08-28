FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/GiG.Core.Template.Web/GiG.Core.Template.Web.csproj", "src/GiG.Core.Template.Web/"]
RUN dotnet restore "src/GiG.Core.Template.Web/GiG.Core.Template.Web.csproj"
COPY . .
WORKDIR "src/GiG.Core.Template.Web"
RUN dotnet build "GiG.Core.Template.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GiG.Core.Template.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GiG.Core.Template.Web.dll"]