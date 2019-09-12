FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview9 as base
ENV DOTNET_ROLL_FORWARD_ON_NO_CANDIDATE_FX=2
WORKDIR /app
ARG VERSION='1.0.0'
COPY . .
RUN dotnet restore GiG.Core.sln
RUN dotnet build GiG.Core.sln /p:Version=$VERSION -c Release --no-restore

FROM base AS publish
ENV SOURCE=https://api.nuget.org/v3/index.json
ENV API_KEY=
COPY docker/scripts/publish.sh .
ENTRYPOINT ./publish.sh

FROM base AS test
ENV ARTIFACTS_DIR="/reports"
VOLUME $ARTIFACTS_DIR
COPY docker/scripts/test.sh .
ENTRYPOINT ./test.sh
