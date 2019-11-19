FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.0.100-0.1.0.2 as base

COPY . .
RUN dotnet restore GiG.Core.sln
RUN dotnet build GiG.Core.sln /p:Version=$VERSION -c Release --no-restore

FROM base AS publish
ENTRYPOINT ["/scripts/publish.sh"]

FROM base AS test
ENTRYPOINT ["/scripts/test.sh"]
CMD ["-u", "-i", "-f", "Category=Unit|Category=Integration", "/src/tests/"]