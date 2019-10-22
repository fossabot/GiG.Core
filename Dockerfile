FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-0.1.0.12.eeebb22 as base

COPY . .
RUN dotnet restore GiG.Core.sln
RUN dotnet build GiG.Core.sln /p:Version=$VERSION -c Release --no-restore

#FROM base AS publish
#ENV SOURCE=https://igcproget.igc.zone/nuget/gig-core-nuget/
#COPY docker/scripts/publish.sh .
#ENTRYPOINT ./publish.sh
#
#FROM base AS test
#ENV ARTIFACTS_DIR="/reports"
#VOLUME $ARTIFACTS_DIR
#COPY docker/scripts/test.sh .
#ENTRYPOINT ./test.sh